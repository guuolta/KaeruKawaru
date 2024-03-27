using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HowToPlayPanelPresenter : SelectPanelPresenterBase<HowToPlayPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetButton(Ct);
    }
    private void SetButton(CancellationToken ct)
    {
        View.LeftButton.OnClickCallback += () => {
            View.SlideLeftAsync(ct).Forget();
        };
        View.RightButton.OnClickCallback += () => {
            View.SlideRightAsync(ct).Forget();
        };
    }
}
