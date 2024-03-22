using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuPanelView : PanelViewBase
{
    [Header("ゲームに戻るボタン")]
    [SerializeField]
    private ButtonBase _returnButton;
    public ButtonBase ReturnButton => _returnButton;
    [Header("サウンド設定ボタン")]
    [SerializeField]
    private ButtonBase _soundSettingButton;
    public ButtonBase SoundSettingButton => _soundSettingButton;
    [Header("リトライボタン")]
    [SerializeField]
    private ButtonBase retryButton;
    public ButtonBase RetryButton => retryButton;
    [Header("タイトルに戻るボタン")]
    [SerializeField]
    private ButtonBase _titleButton;
    public ButtonBase TitleButton => _titleButton;
}
