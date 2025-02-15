public class PauseSoundPanelPresenter : SoundPanelPresenterBase
{
    protected override void SetEvent()
    {
        base.SetEvent();
        // 閉じるボタンの設定
        PausePanelManager.Instance.SetEventCloseButton(View.CloseButton, destroyCancellationToken);
    }
}
