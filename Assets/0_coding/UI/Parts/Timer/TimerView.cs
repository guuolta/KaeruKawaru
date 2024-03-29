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

    private Dictionary<TimerState, Color> _timerColorDic = new Dictionary<TimerState, Color>();
    private ReactiveProperty<TimerState> _timeState = new ReactiveProperty<TimerState>(TimerState.Normal);
    private Sequence _sequence;

    protected override void Init()
    {
        base.Init();
        _sequence = DOTween.Sequence();
        AddTween(_sequence);
        _timerGauge.fillAmount = 0;
        _needle.rectTransform.localEulerAngles = new Vector3(0, 0, 0);
        GetDictionary();
    }

    private void GetDictionary()
    {
        foreach (var timerColor in _timerColorList)
            _timerColorDic.Add(timerColor.TimerState, timerColor.TimerColor);
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventState(Ct);
    }

    /// <summary>
    /// タイマーの状態によるイベントの設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventState(CancellationToken ct)
    {
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
    /// タイマーの色を設定
    /// </summary>
    /// <param name="state"> タイマーの状態 </param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask SetTimerColorAsync(TimerState state, CancellationToken ct)
    {
        var _timerColorSequence = DOTween.Sequence();
        AddTween(_timerColorSequence);

        await _timerColorSequence
            .Append(_timerGauge
                .DOColor(_timerColorDic[state], AnimationTime)
                .SetEase(Ease.InSine))
            .Join(_timerText.DOColor(_timerColorDic[state], AnimationTime)
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
    /// <param name="maxTime"> 開始時間 </param>
    /// <param name="animationTime"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask SetTimerAsync(int time, int maxTime, int animationTime, CancellationToken ct)
    {
        _timerGauge.DOComplete();
        _needle.rectTransform.DOComplete();
        _timerText.rectTransform.DOComplete();

        float fillAmount = (float)time / maxTime;

        await _sequence
            .Append(_timerGauge
                .DOFillAmount(1 - fillAmount, animationTime)
                .SetEase(Ease.Linear))
            .Join(_needle.rectTransform
                .DORotate(new Vector3(0, 0, fillAmount * 360), animationTime)
                .SetEase(Ease.Linear))
            .ToUniTask(cancellationToken: ct);

        if (_timeState.Value == TimerState.Danger)
        {
            var _textSequence = DOTween.Sequence();
            AddTween(_textSequence);

            _textSequence
                .Append(_timerText.rectTransform
                    .DOScale(Vector3.zero, AnimationTime / 3)
                    .SetEase(Ease.OutSine))
                .Append(_timerText.rectTransform
                    .DOScale(Vector3.one * 1.2f, AnimationTime / 3)
                    .SetEase(Ease.InSine))
                .Append(_timerText.rectTransform
                    .DOScale(Vector3.one, AnimationTime / 3)
                    .SetEase(Ease.OutSine))
                .ToUniTask(cancellationToken: ct)
                .Forget();
        }

        _timerText.text = time.ToString();
    }

    public override UniTask ShowAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }

    public override UniTask HideAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }
}

/// <summary>
/// タイマーのデータ
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

public enum TimerState
{
    Normal,
    Warning,
    Danger
}