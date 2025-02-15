using UniRx;

/// <summary>
/// ステージ選択パネル
/// </summary>
public class StageSelectPanelPresenter : SelectPanelPresenterBase<StageSelectPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetButton();
    }

    /// <summary>
    /// ボタン押したときのイベント
    /// </summary>
    private void SetButton()
    {
        // レベル選択
        View.EasyButton.OnClickEvent
            .Subscribe(_ => 
            {
                GameSceneManager.LoadScene(SceneType.EasyGame);
            });
        View.HardButton.OnClickEvent
            .Subscribe(_ =>
            {
                GameSceneManager.LoadScene(SceneType.HardGame);
            });
    }
}
