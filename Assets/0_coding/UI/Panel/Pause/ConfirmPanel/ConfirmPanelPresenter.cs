using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// 確認ダイアログ
/// </summary>
public class ConfirmPanelPresenter : PanelPresenterBase<ConfirmPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventButton(Ct);
    }

    /// <summary>
    /// ボタンのイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventButton(CancellationToken ct)
    {
        // イエスボタンでタイトルに戻る
        View.YesButton.OnClickCallback += () =>
        {
            GameSceneManager.LoadScene(SceneType.Title);
        };

        // Noボタンでダイアログを非表示
        View.NoButton.OnClickCallback += () =>
        {
            PausePanelManager.Instance.ClosePanelAsync(ct).Forget();
        };
    }
}
