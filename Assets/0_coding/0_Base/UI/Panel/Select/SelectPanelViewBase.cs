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
    
    public override void Init()
    {
        base.Init();
        
        _closeButton.Init();
    }
}
