using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TitleManager : SingletonObjectBase<TitleManager>
{
    protected override void Init()
    {
        base.Init();
        AudioManager.Instance.PlayBGM(BGMType.Title);
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventStart();
    }

    /// <summary>
    /// ゲームを始めるイベント設定
    /// </summary>
    private void SetEventStart()
    {
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ =>
            {
                /*削除して、セレクト画面に遷移する*/
                GameSceneManager.LoadScene(SceneType.EasyGame);
            });
    }
}
