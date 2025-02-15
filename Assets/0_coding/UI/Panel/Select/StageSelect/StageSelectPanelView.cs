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
    
    public override void Init()
    {
        base.Init();
        
        // ボタン初期化
        _easyButton.Init();
        _hardButton.Init();
    }
}
