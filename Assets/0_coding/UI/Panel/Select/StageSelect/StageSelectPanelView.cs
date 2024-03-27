using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectPanelView : SelectPanelViewBase
{
    [Header("イージーボタン")]
    [SerializeField]
    private ButtonBase _easyButton;
    public ButtonBase EasyButton => _easyButton;

    [Header("ハードボタン")]
    [SerializeField]
    private ButtonBase _hardButton;
    public ButtonBase HardButton => _hardButton;
}
