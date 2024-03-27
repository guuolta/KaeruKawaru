using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SelectPanelPresenter : PanelPresenterBase<SelectPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetButton(Ct);
    }
    private void SetButton(CancellationToken ct)
    {
        View.StageSelectButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.StageSelect,ct).Forget();
        };
        View.SoundButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Sound,ct).Forget();
        };
        View.RankButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Score,ct).Forget();
        };
        View.CreditButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Credit,ct).Forget();
        };
        View.HowPlayButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.HowToPlay,ct).Forget();
        };
        View.CloseButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Title,ct).Forget();
        };
    }
}
