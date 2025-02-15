using UniRx;
using UnityEngine;

public class TimerModel
{
    private ReactiveProperty<int> _timeValue;
    /// <summary>
    /// 経過時間
    /// </summary>
    public IReadOnlyReactiveProperty<int> TimeValue => _timeValue;
    public int MaxTime => StageManager.Instance.TimeLimit;
    float _leftTime;
    private CompositeDisposable _disposable;

    public TimerModel()
    {
        _timeValue = new ReactiveProperty<int>(0);
        _disposable = new CompositeDisposable();
        
        SetEventTime();
    }
    
    public void SetEventTime()
    {
        Observable.EveryUpdate()
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

    public void DispoiseTimerEvent()
    {
        _disposable.Dispose();
        _disposable = new CompositeDisposable();
    }
}
