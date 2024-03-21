using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Timer
    {
        private readonly ReactiveProperty<int> _left;
        private readonly Subject<Unit> _overSubject = new Subject<Unit>();
        private IDisposable _subscription;

        public int Max { get; }
        public IReactiveProperty<int> Left => _left;
        public IObservable<Unit> OverObservable => _overSubject;

        public Timer(int max)
        {
            Max = max;
            _left = new ReactiveProperty<int>(max);
            _subscription = null;
        }

        public void Start()
        {
            if (_subscription != null) return;

            // 更新処理
            _subscription = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    // 時間を減らす
                    _left.Value -= (int)Mathf.Floor(Time.deltaTime);
                });

            // 残り時間が0になったらTimeOverイベントを発行
            _left
                .Where(value => value <= 0)
                .Subscribe(_ => { _overSubject.OnNext(Unit.Default); });
        }

        public void Stop()
        {
            _subscription?.Dispose();
            _subscription = null;
        }

        public void Reset()
        {
            // 残り時間のリセット
            _left.Value = Max;
        }
    }

/*
public class TPbeta : PresenterBase<TView>
    where TView : ViewBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}/**/
