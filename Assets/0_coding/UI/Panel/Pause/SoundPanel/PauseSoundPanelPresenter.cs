public class PauseSoundPanelPresenter : SoundPanelPresenterBase
{
    protected override void SetEvent()
    {
        base.SetEvent();
        PausePanelManager.Instance.SetEventCloseButton(View.CloseButton, Ct);
    }
}
