using UnityEngine;

public class SelectPanelViewBase : PanelViewBase
{
    [Header("閉じるボタン")]
    [SerializeField]
    private ButtonBase _closeButton;
    /// <summary>
    /// 閉じるボタン
    /// </summary>
    public ButtonBase CloseButton => _closeButton;
}
