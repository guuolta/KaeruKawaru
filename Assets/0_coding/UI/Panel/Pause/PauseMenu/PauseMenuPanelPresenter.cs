using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class PauseMenuPanelPresenter : PanelPresenterBase<PauseMenuPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventClick(Ct);
    }

    /// <summary>
    /// クリックイベント設定
    /// </summary>
    private void SetEventClick(CancellationToken ct)
    {
        View.ReturnButton.OnClickCallback += () =>
        {
            PausePanelManager.Instance.ClosePanelAsync(ct).Forget();
        };
        View.SoundSettingButton.OnClickCallback += () =>
        {
            PausePanelManager.Instance.OpenPanelAsync(PausePanelType.Sound, ct).Forget();
        };
        View.RetryButton.OnClickCallback += () =>
        {
            GameSceneManager.ReLoadSceneAsync().Forget();
        };
        View.TitleButton.OnClickCallback += () =>
        {
            PausePanelManager.Instance.OpenPanelAsync(PausePanelType.Confirm, ct).Forget();
        };
    }
}
