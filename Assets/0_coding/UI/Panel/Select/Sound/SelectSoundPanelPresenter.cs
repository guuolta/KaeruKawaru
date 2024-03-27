using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SelectSoundPanelPresenter : SoundPanelPresenterBase
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetButton(Ct);
    }

    private void SetButton(CancellationToken ct)
    {
        View.CloseButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Slect,ct).Forget();
        };
    }
}
