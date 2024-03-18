using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerOperator : ObjectBase
{
    [Header("クリックのクールタイム")]
    [SerializeField]
    private float _clickInterval = 0.1f;
    CompositeDisposable _disposable = new CompositeDisposable();

    /// <summary>
    /// クリックしたときのイベント発行
    /// </summary>
    public void SetEventClick()
    {
        Observable.EveryUpdate() // 毎フレーム
            .TakeUntilDestroy(this) // このクラスが破棄されるまで
            .Where(_ => Input.GetMouseButtonDown(0)) // マウスの左クリックがされたとき
            .ThrottleFirst(TimeSpan.FromSeconds(_clickInterval)) // クリックのクールタイム
            .DistinctUntilChanged() // 直前の値と同じなら発行しない
            .Subscribe(_ =>
            {
                //レイキャストでFrogを取得
                //その後、FrogのEvolveメソッドを呼び,_disposableを破棄
            }).AddTo(_disposable);
    }
}
