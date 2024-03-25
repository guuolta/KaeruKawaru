using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class TitleManager : SingletonObjectBase<TitleManager>
{
    private CompositeDisposable _disposable = new CompositeDisposable();
    protected override void SetEvent()
    {
        base.SetEvent();

        AudioManager.Instance.PlayBGM(BGMType.Title);
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ =>
            {
                GameSceneManager.LoadScene(SceneType.EasyGame);
            });

    }

    //protected override void SetEvent()
    //{
    //    base.SetEvent();
    //    SetEventGameState(Ct);
    //}

    ///// <summary>
    ///// ゲームを始めるイベント設定
    ///// </summary>
    //private void SetEventStart()
    //{
    //    Observable.EveryUpdate()
    //        .TakeUntilDestroy(this)
    //        .Where(_ => Input.GetMouseButtonDown(0))
    //        .Subscribe(_ => {
    //            GameStateManager.SetGameState(GameState.Select);
    //        }).AddTo(_disposable);
    //}
    //private void SetEventGameState(CancellationToken ct)
    //{
    //    GameStateManager.Status
    //        .TakeUntilDestroy(this)
    //        .Where(_ => GameStateManager.Status.Value != GameState.Load)
    //        .Select(_ => _ == GameState.Select)
    //        .DistinctUntilChanged()
    //        .Subscribe(async _ => {
    //            if(_){
    //                _disposable = DisposeEvent(_disposable);
    //                await SelectPanelManager.Instance.OpenFirstPanelAsync(ct);
    //            }
    //            else{
    //                SetEventStart();
    //            }
    //        });
    //}
}
