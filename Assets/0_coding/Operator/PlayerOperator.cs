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

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventState();
    }

    private void SetEventState()
    {
        GameStateManager.Status
            .TakeUntilDestroy(this)
            .Select(value => value == GameState.Play)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if(value)
                {
                    SetEventClick();
                }
                else
                {
                    _disposable = DisposeEvent(_disposable);
                }
            });
    }

    /// <summary>
    /// クリックしたときのイベント発行
    /// </summary>
    public void SetEventClick()
    {
        Observable.EveryUpdate() // 毎フレーム
            .TakeUntilDestroy(this) // このクラスが破棄されるまで
            .Where(_ => Input.GetMouseButtonDown(0) && QuestionManager.Instance.IsCheckedAnswer.Value) // マウスの左クリックがされて、ステージのチェックが終わったとき
            .DistinctUntilChanged() // 直前の値と同じなら発行しない
            .ThrottleFirst(TimeSpan.FromSeconds(_clickInterval)) // クリックのクールタイム
            .Subscribe(_ =>
            {
                //レイキャストでFrogを取得
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var frog = hit.collider.GetComponent<Frog>();
                    if(frog == null)
                        return;
                    
                    frog.Evolve();
                }          
            }).AddTo(_disposable);
    }
}
