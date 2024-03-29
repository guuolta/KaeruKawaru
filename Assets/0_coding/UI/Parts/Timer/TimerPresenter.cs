using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

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

    private TimerModel _model;
    private int _maxTime => _model.MaxTime;

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
        SetEventTimer(Ct);
    }

    private void SetEventTimer(CancellationToken ct)
    {
        GetChangeTimeList();
        View.ChangeTimerState(TimerState.Normal);
        View.SetTimerAsync(_maxTime, _maxTime, 0, ct).Forget();

        _model.TimeValue
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Skip(1)
            .Select(value => _maxTime - value)
            .Subscribe(async value =>
            {
                await View.SetTimerAsync(value, _maxTime, _animationTime, ct);

                if (value <= 0)
                {
                    GameStateManager.SetGameState(GameState.Result);
                    DisposeEvent(_disposable);
                }
                else if (value == _changeTimeList[2])
                {
                    View.ChangeTimerState(TimerState.Danger);
                }
                else if(value == _changeTimeList[1])
                {
                    View.ChangeTimerState(TimerState.Warning);
                }
                else if (value == _changeTimeList[0])
                {
                    View.ChangeTimerState(TimerState.Normal);
                }

            }).AddTo(_disposable);
    }

    private void GetChangeTimeList()
    {
        foreach(var percentage in _timerPercentageList)
        {
            _changeTimeList.Add(_maxTime - _maxTime * percentage / 100);
        }
    }
}