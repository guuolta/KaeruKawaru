using System;
using UniRx;
using UnityEngine;

/// <summary>
///プレイヤーの操作の管理
/// </summary>
public class PlayerOperator : SingletonObjectBase<PlayerOperator>
{
    [Header("クリックのクールタイム")]
    [SerializeField]
    private float _clickInterval = 0.1f;

    private ReactiveProperty<int> _clickCount = new ReactiveProperty<int>(0);
    /// <summary>
    /// クリック回数
    /// </summary>
    public IReadOnlyReactiveProperty<int> ClickCount => _clickCount;

    CompositeDisposable _disposable = new CompositeDisposable();

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        SetEvent();
    }
    
    /// <summary>
    /// イベント発行
    /// </summary>
    private void SetEvent()
    {
        SetEventState();
    }

    /// <summary>
    /// ステートの変化時のイベント発行
    /// </summary>
    private void SetEventState()
    {
        // ゲーム状態ならクリックできるようにし、そうでないならクリックできなくする
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
                    _disposable.Dispose();
                    _disposable = new CompositeDisposable();
                }
            });
    }

    /// <summary>
    /// クリックしたときのイベント発行
    /// </summary>
    private void SetEventClick()
    {
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetMouseButtonDown(0) && QuestionManager.Instance.IsCheckedAnswer.Value) // お題のクリア判定が終わるまでは、クリックできないようにする
            .DistinctUntilChanged()
            .ThrottleFirst(TimeSpan.FromSeconds(_clickInterval)) // 連打防止
            .Subscribe(_ =>
            {
                // レイを飛ばして、ヒットしたものがカエルのオブジェクトなら、進化させる
                // クリック回数もカウント
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit))
                {
                    var frog = hit.collider.GetComponent<Frog>();
                    if(frog == null)
                        return;
                    
                    frog.Evolve();
                    _clickCount.Value++;
                }          
            }).AddTo(_disposable);
    }
}
