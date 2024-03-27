using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPanelViewBase : PanelViewBase
{
    [Header("閉じるボタン")]
    [SerializeField]
    private ButtonBase _closeButton;
    public ButtonBase CloseButton => _closeButton;
}
