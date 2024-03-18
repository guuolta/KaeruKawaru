using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuPanelView : PanelViewBase
{
    [Header("サウンド設定ボタン")]
    [SerializeField]
    private ButtonBase _soundSettingButton;
    public ButtonBase SoundSettingButton => _soundSettingButton;
    [Header("マウス設定ボタン")]
    [SerializeField]
    private ButtonBase _mouseSettingButton;
    public ButtonBase MouseSettingButton => _mouseSettingButton;
    [Header("クレジットボタン")]
    [SerializeField]
    private ButtonBase _creditButton;
    public ButtonBase CreditButton => _creditButton;
}
