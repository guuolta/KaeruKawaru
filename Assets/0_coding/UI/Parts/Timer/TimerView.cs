using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TimerView : ViewBase
{
    [Header("タイマーゲージ")]
    [SerializeField]
    private Image _timerGauge;
    [Header("針")]
    [SerializeField]
    private Image _needle;
    [Header("タイマーテキスト")]
    [SerializeField]
    private TMP_Text _timerText;
    [Header("タイマーの色")]
    [SerializeField]
    private List<TimerColorData> _timerColorList = new List<TimerColorData>();
    
    private int _startTime;
    /// <summary>
    /// タイマーの色の辞書
    /// </summary>
    private Dictionary<TimerState, Color> _timerColorDic = new Dictionary<TimerState, Color>();
    /// <summary>
    /// タイマーの状態
    /// </summary>
    private ReactiveProperty<TimerState> _timeState = new ReactiveProperty<TimerState>(TimerState.Normal);
    
    private Sequence _sequence;

    public override void Init()
    {
        // アニメーションの初期化
        _sequence = DOTween.Sequence();
        
        // タイマーの針とゲージの初期化
        _timerGauge.fillAmount = 0;
        _needle.rectTransform.localEulerAngles = new Vector3(0, 0, 0);
        
        InitColorDictionary();
        
        base.Init();
    }

    /// <summary>
    /// タイマーの色の辞書の初期化
    /// </summary>
    private void InitColorDictionary()
    {
        foreach (var timerColor in _timerColorList)
            _timerColorDic.Add(timerColor.TimerState, timerColor.TimerColor);
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventState(destroyCancellationToken);
    }

    /// <summary>
    /// タイマーの状態によるイベントの設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventState(CancellationToken ct)
    {
        // タイマーの状態が変化したら対応する色に変える
        _timeState
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(async state =>
            {
                await SetTimerColorAsync(state, ct);
            });

        GameStateManager.Status
            .TakeUntilDestroy(this)
            .Select(value => value == GameState.Pause)
            .DistinctUntilChanged()
            .Skip(1)
            .Subscribe(value =>
            {
                if (value)
                {
                    _sequence.Pause();
                }
                else
                {
                    _sequence.Play();
                }
            });
    }

    /// <summary>
    /// 開始タイム
    /// </summary>
    /// <param name="startTime"></param>
    public void SetMaxTime(int startTime)
    {
        _startTime = startTime;
        _timerText.text = startTime.ToString();
    }

    /// <summary>
    /// タイマーの色を設定
    /// </summary>
    /// <param name="state"> タイマーの状態 </param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask SetTimerColorAsync(TimerState state, CancellationToken ct)
    {
        var timerColorSequence = DOTween.Sequence();

        // ゲージとテキストの色を変える
        await timerColorSequence
            .Append(_timerGauge
                .DOColor(_timerColorDic[state], AnimationSec)
                .SetEase(Ease.InSine))
            .Join(_timerText.DOColor(_timerColorDic[state], AnimationSec)
                .SetEase(Ease.InSine))
            .ToUniTask(cancellationToken: ct);
    }

    /// <summary>
    /// タイマーの状態を変更
    /// </summary>
    /// <param name="state"></param>
    public void ChangeTimerState(TimerState state)
    {
        _timeState.Value = state;
    }

    /// <summary>
    /// タイマーを設定
    /// </summary>
    /// <param name="time"> 設定する時間 </param>
    /// <param name="animationSec"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask SetTimerAsync(int time, int animationSec, CancellationToken ct)
    {
        // ゲージの値
        float fillAmount = (float)(time-1) / _startTime;

        // Danger状態ならテキストを拡大縮小するアニメーションをする
        if (_timeState.Value == TimerState.Danger)
        {
            var textSequence = DOTween.Sequence().SetLink(GameObject);
            _timerText.rectTransform.DOComplete();

            textSequence
                .Append(_timerText.rectTransform
                    .DOScale(Vector3.zero, animationSec / 3)
                    .SetEase(Ease.OutSine))
                .Append(_timerText.rectTransform
                    .DOScale(Vector3.one * 1.2f, animationSec / 3)
                    .SetEase(Ease.InSine))
                .Append(_timerText.rectTransform
                    .DOScale(Vector3.one, animationSec / 3)
                    .SetEase(Ease.OutSine))
                .ToUniTask(cancellationToken: ct).Forget();
        }

        // ゲージと針を進める
        if (fillAmount >= 0)
        {
            _sequence.Complete();
            _sequence = DOTween.Sequence();

            await _sequence
            .Append(_timerGauge
                .DOFillAmount(1 - fillAmount, animationSec)
                .SetEase(Ease.Linear))
            .Join(_needle.rectTransform
                .DORotate(new Vector3(0, 0, fillAmount * 360), animationSec)
                .SetEase(Ease.Linear))
            .ToUniTask(cancellationToken: ct);
        }
        
        // 時間のテキストを更新
        _timerText.text = time.ToString();
    }
}

/// <summary>
/// タイマーの色データ
/// </summary>
[System.Serializable]
public class TimerColorData
{
    [Header("タイマーの状態")]
    [SerializeField]
    private TimerState _timerState;
    /// <summary>
    /// タイマーの状態
    /// </summary>
    public TimerState TimerState => _timerState;
    [Header("タイマーの色")]
    [SerializeField]
    private Color _timerColor;
    /// <summary>
    /// タイマーの色
    /// </summary>
    public Color TimerColor => _timerColor;
}

/// <summary>
/// タイマーの状態
/// </summary>
public enum TimerState
{
    Normal,
    Warning,
    Danger
}