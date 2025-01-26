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
        View.EasyButton.OnClickCallback += () => {
            GameSceneManager.LoadScene(SceneType.EasyGame);
        };
        View.HardButton.OnClickCallback += () => {
            GameSceneManager.LoadScene(SceneType.HardGame);
        };
    }
}
