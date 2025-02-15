using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

/// <summary>
/// 確認ダイアログ
/// </summary>
public class ConfirmPanelPresenter : PanelPresenterBase<ConfirmPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventButton(destroyCancellationToken);
    }

    /// <summary>
    /// ボタンのイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventButton(CancellationToken ct)
    {
        // イエスボタンでタイトルに戻る
        View.YesButton.OnClickEvent
            .Subscribe(_ =>
            {
                GameSceneManager.LoadScene(SceneType.Title);
            });

        // Noボタンでダイアログを非表示
        View.NoButton.OnClickEvent
            .Subscribe(_ =>
            {
                PausePanelManager.Instance.ClosePanelAsync(ct).Forget();
            });
    }
}
