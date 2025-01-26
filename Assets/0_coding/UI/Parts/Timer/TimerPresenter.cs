using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

/// <summary>
/// タイマー
/// </summary>
public class TimerPresenter : PresenterBase<TimerView>
{
    [Header("タイマーのアニメーションの時間")]
    [SerializeField]
    private int _animationTime = 1;
    [Header("タイマーの色を変えるパーセンテージ")]
    [Range(0, 100)]
    [SerializeField]
    private List<int> _timerPercentageList = new List<int>
    {
        0,
        50,
        75
    };

    [Header("加速SE")]
    [SerializeField]
    private AudioClip _hurryupSE;
    private TimerModel _model;
    private int _startTime => _model.MaxTime;

    private List<int> _changeTimeList = new List<int>();
    private CompositeDisposable _disposable = new CompositeDisposable();
    

    protected override void Init()
    {
        base.Init();
        _model = GetComponent<TimerModel>();

        if(_model == null)
        {
            _model = GameObject.AddComponent<TimerModel>();
        }
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        View.SetMaxTime(_startTime);
        SetEventTimer(Ct);
        SetEventDoTimer();
    }
    
    /// <summary>
    /// タイマー開始のイベント
    /// </summary>
    private void SetEventDoTimer()
    {
        // ゲームプレイ中はタイマーを進める
        // ゲームプレイ中でない場合は、タイマーを止める
        GameStateManager.Status
            .TakeUntilDestroy(this)
            .Select(value => value == GameState.Play)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if (value)
                {
                    _model.SetEventTime();
                }
                else
                {
                    _model.DispoiseTimerEvent();
                }
            });
    }

    /// <summary>
    /// タイマーの時間のイベント
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventTimer(CancellationToken ct)
    {
        GetChangeTimeList();
        // タイマーの見た目初期化
        View.ChangeTimerState(TimerState.Normal);

        // タイマーの時間が減った時、見た目を変える
        _model.TimeValue
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Select(value => _startTime - value)
            .Where(value => value >= 0)
            .Subscribe(async value =>
            {
                // ゲームプレイ中でない場合は、停止
                await UniTask.WaitUntil(() => GameStateManager.Status.Value == GameState.Play, cancellationToken: ct);
                // Viewに現在の経過時間を知らせる
                await View.SetTimerAsync(value, _animationTime, ct);

                // 0秒の時リザルトへ
                if (value <= 0)
                {
                    AudioManager.Instance.ChangeBGMPitch(1f);
                    GameStateManager.SetGameState(GameState.Result);
                    DisposeEvent(_disposable);
                }
                // タイマーの色を変える時間になったら対応する色にする
                else if (value == _changeTimeList[2])
                {
                    View.ChangeTimerState(TimerState.Danger);
                    AudioManager.Instance.ChangeBGMPitch(0.8f); // ピッチを低くする
                    AudioManager.Instance.PlayOneShotSE(_hurryupSE); // SEを鳴らす
                }
                else if (value == _changeTimeList[1])
                {
                    View.ChangeTimerState(TimerState.Warning);
                }
                else if (value == _changeTimeList[0])
                {
                    View.ChangeTimerState(TimerState.Normal);
                }

            }).AddTo(_disposable);
    }

    /// <summary>
    /// タイマーの色を変える時間のリストを取得
    /// </summary>
    private void GetChangeTimeList()
    {
        foreach(var percentage in _timerPercentageList)
        {
            _changeTimeList.Add(_startTime - _startTime * percentage / 100);
        }
    }
}