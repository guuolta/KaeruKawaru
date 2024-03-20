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
            .ThrottleFirst(TimeSpan.FromSeconds(_clickInterval)) // クリックのクールタイム
            .Where(_ => Input.GetMouseButtonDown(0) && QuestionManager.Instance.IsCheckedAnswer.Value) // マウスの左クリックがされて、ステージのチェックが終わったとき
            .DistinctUntilChanged() // 直前の値と同じなら発行しない
            .Subscribe(value =>
            {
                //レイキャストでFrogを取得
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit,15.0f))
                {
                    Debug.Log(hit.collider.gameObject.name);
                }
                //その後、FrogのEvolveメソッドを呼ぶ
            }).AddTo(_disposable);
    }
}
