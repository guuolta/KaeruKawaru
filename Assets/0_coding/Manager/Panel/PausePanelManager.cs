using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PausePanelManager : PanelManagerBase<PausePanelManager>
{
    [Header("背景画像")]
    [SerializeField]
    private UIBase _backgroundImage;
    [Header("ポーズボタン")]
    [SerializeField]
    private ButtonBase _pauseButton;
    [Header("ポーズメニューパネル")]
    [SerializeField]
    private PauseMenuPanelPresenter _pauseMenuPanel;
    [Header("サウンドパネル")]
    [SerializeField]
    private SoundPanelPresenterBase _soundPanel;
    [Header("確認パネル")]
    [SerializeField]
    private ConfirmPanelPresenter _confirmPanel;

    protected override void Init()
    {
        base.Init();
        _backgroundImage.ChangeInteractive(false);
        SetFirstPanel(_pauseMenuPanel);
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventPauseButton();
        SetEventPanel(Ct);
    }

    /// <summary>
    /// ポーズボタンのイベント設定
    /// </summary>
    private void SetEventPauseButton()
    {
        _pauseButton.ChangeInteractive(false);

        GameStateManager.Status
            .TakeUntilDestroy(this)
            .Where(value => value == GameState.Play)
            .Take(1)
            .Subscribe(_ =>
            {
                _pauseButton.ChangeInteractive(true);
            });

        _pauseButton.OnClickCallback += () =>
        {
            if(GameStateManager.Status.Value == GameState.Play)
            {
                GameStateManager.SetGameState(GameState.Pause);
            }
        };
    }

    /// <summary>
    /// パネルを開くイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventPanel(CancellationToken ct)
    {
        GameStateManager.Status
            .TakeUntilDestroy(this)
            .Where(value => value == GameState.Pause)
            .Subscribe(async value =>
            {
                await OpenFirstPanelAsync(ct);
                _backgroundImage.ChangeInteractive(true);
            });
    }

    /// <summary>
    /// パネルを開く
    /// </summary>
    /// <param name="type"> パネルの種類 </param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask OpenPanelAsync(PausePanelType type, CancellationToken ct)
    {
        switch (type)
        {
            case PausePanelType.PauseMenu:
                await OpenFirstPanelAsync(ct);
                break;
            case PausePanelType.Sound:
                await OpenPanelAsync(_soundPanel, ct);
                break;
            case PausePanelType.Confirm:
                await OpenPanelAsync(_confirmPanel, ct);
                break;
        }
    }

    public override async UniTask ClosePanelAsync(CancellationToken ct)
    {
        await base.ClosePanelAsync(ct);

        if(TargetPanel == null)
        {
            _backgroundImage.ChangeInteractive(false);
            GameStateManager.SetGameState(GameState.Play);
        }
    }
}

/// <summary>
/// ポーズパネルの種類
/// </summary>
public enum PausePanelType
{
    None,
    PauseMenu,
    Sound,
    Confirm
}