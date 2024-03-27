using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SelectPanelPresenterBase<TView> : PanelPresenterBase<TView>
    where TView : SelectPanelViewBase
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetCloseButton(Ct);
    }

    private void SetCloseButton(CancellationToken ct)
    {
        View.CloseButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Slect,ct).Forget();
        };
    }
}
