using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TimerModel : ObjectBase
{
    private ReactiveProperty<int> _timeValue = new ReactiveProperty<int>(0);
    /// <summary>
    /// 経過時間
    /// </summary>
    public IReadOnlyReactiveProperty<int> TimeValue => _timeValue;
    public int MaxTime => StageManager.Instance.TimeLimit;
    float _leftTime;
    private CompositeDisposable _disposable = new CompositeDisposable();

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventDoTimer();
    }
    
    private void SetEventDoTimer()
    {
        GameStateManager.Status
            .TakeUntilDestroy(this)
            .Select(value => value == GameState.Play)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if (value)
                {
                    SetEventTime();
                }
                else
                {
                    _disposable = DisposeEvent(_disposable);
                }
            });
    }

    private void SetEventTime()
    {
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Select(_ => Time.deltaTime)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                _leftTime += value;

                if (_leftTime >= 1)
                {
                    _timeValue.Value++;
                    _leftTime -= 1;
                }
            }).AddTo(_disposable);
    }
}
