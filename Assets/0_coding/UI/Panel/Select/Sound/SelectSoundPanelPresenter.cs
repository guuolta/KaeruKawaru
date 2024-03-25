using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSoundPanelPresenter : SoundPanelPresenterBase
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SelectPanelManager.Instance.SetEventCloseButton(View.CloseButton,Ct);
    }
}
