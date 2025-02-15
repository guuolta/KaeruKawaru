using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

public class PauseMenuPanelPresenter : PanelPresenterBase<PauseMenuPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventButton(destroyCancellationToken);
    }

    /// <summary>
    /// ボタンのイベント設定
    /// </summary>
    private void SetEventButton(CancellationToken ct)
    {
        // ゲームに戻る
        View.ReturnButton.OnClickEvent
            .Subscribe( _ =>
            {
                PausePanelManager.Instance.ClosePanelAsync(ct).Forget();
            });
        
        // 音量調節
        View.SoundSettingButton.OnClickEvent
            .Subscribe(_ =>
            {
                PausePanelManager.Instance.OpenPanelAsync(PausePanelType.Sound, ct).Forget();
            });
        
        // リトライ
        View.RetryButton.OnClickEvent
            .Subscribe(_ =>
            {
                GameSceneManager.ReLoadSceneAsync().Forget();
            });
        
        // タイトルへ
        View.TitleButton.OnClickEvent
            .Subscribe(_ =>
            {
                PausePanelManager.Instance.OpenPanelAsync(PausePanelType.Confirm, ct).Forget();
            });
    }
}
