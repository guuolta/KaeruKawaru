using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SelectPanelView : PanelViewBase
{
    [Header("ステージ選択ボタン")]
    [SerializeField]
    private TextButton _stageSelectButton;
    public TextButton StageSelectButton => _stageSelectButton;

    [Header("サウンドボタン")]
    [SerializeField]
    private ButtonBase _soundButton;
    public ButtonBase SoundButton => _soundButton;

    [Header("ランクボタン")]
    [SerializeField]
    private ButtonBase _rankButton;
    public ButtonBase RankButton => _rankButton;

    [Header("クレジットボタン")]
    [SerializeField]
    private ButtonBase _creditButton;
    public ButtonBase CreditButton => _creditButton;

    [Header("遊び方ボタン")]
    [SerializeField]
    private ButtonBase _howPlayButton;
    public ButtonBase HowPlayButton => _howPlayButton;

    [Header("閉じるボタン")]
    [SerializeField]
    private ButtonBase _closeButton;
    public ButtonBase CloseButton => _closeButton;

    protected override void Init()
    {
        base.Init();
        HideButton();
    }
    public override async UniTask ShowAsync(CancellationToken ct)
    {
        await base.ShowAsync(ct);
        ShowButton();
    }
    private void ShowButton()
    {
        _stageSelectButton.ChangeInteractive(true);
        _soundButton.ChangeInteractive(true);
        _rankButton.ChangeInteractive(true);
        _creditButton.ChangeInteractive(true);
        _howPlayButton.ChangeInteractive(true);
    }
    public override async UniTask HideAsync(CancellationToken ct)
    {
        await base.HideAsync(ct);
        HideButton();
    }
    private void HideButton()
    {
        _stageSelectButton.ChangeInteractive(false);
        _soundButton.ChangeInteractive(false);
        _rankButton.ChangeInteractive(false);
        _creditButton.ChangeInteractive(false);
        _howPlayButton.ChangeInteractive(false);
    }

    
}
