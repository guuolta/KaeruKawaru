using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectPanelPresenter : SelectPanelPresenterBase<StageSelectPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetButton();
    }

    private void SetButton()
    {
        View.EasyButton.OnClickCallback += () => {
            GameSceneManager.LoadScene(SceneType.EasyGame);
        };
        View.HardButton.OnClickCallback += () => {
            GameSceneManager.LoadScene(SceneType.HardGame);
        };
    }
}
