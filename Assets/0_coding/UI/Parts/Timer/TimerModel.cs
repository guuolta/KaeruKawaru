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

    public void AddTime(int time)
    {
        _timeValue.Value += time;
    }

    public void SetEventTime()
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

    public void DispoiseTimerEvent()
    {
        _disposable = DisposeEvent(_disposable);
    }
}
