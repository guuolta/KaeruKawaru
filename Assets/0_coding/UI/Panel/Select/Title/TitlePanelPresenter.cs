using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TitlePanelPresenter : PanelPresenterBase<TitlePanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventClick(Ct);
    }

    private void SetEventClick(CancellationToken ct)
    {
        View.OnClickCallback += async () =>
        {
            AudioManager.Instance.PlayOneShotSE(SEType.Posi);
            await TitleManager.Instance.TargetSelectAsync(ct);
            await SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Slect, ct);
        };
    }
}
