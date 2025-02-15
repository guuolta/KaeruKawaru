using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class PausePanelManager : PanelManagerBase<PausePanelManager>
{
    [FormerlySerializedAs("_backgroundImage")]
    [Header("背景画像")]
    [SerializeField]
    private CanvasGroup _background;
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

    public override void Init()
    {
        // ボタン初期化
        _pauseButton.Init();
        
        // パネル初期化
        _pauseMenuPanel.Init();
        _soundPanel.Init();
        _confirmPanel.Init();
        
        // 背景初期化
        ChangeIteactive(false);
        
        SetFirstPanel(_pauseMenuPanel);

        base.Init();
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        
        SetEventPauseButton();
        SetEventPanel(destroyCancellationToken);
    }

    /// <summary>
    /// ポーズボタンのイベント設定
    /// </summary>
    private void SetEventPauseButton()
    {
        _pauseButton.ChangeInteractive(false);

        // ゲームが始まったらポーズボタンを押せるようにする
        GameStateManager.Status
            .TakeUntilDestroy(this)
            .Where(value => value == GameState.Play)
            .Take(1)
            .Subscribe(_ =>
            {
                _pauseButton.ChangeInteractive(true);
            });

        // ポーズボタンを押したらポーズ状態にする
        _pauseButton.OnClickEvent
            .Subscribe(_ =>
            {
                if(GameStateManager.Status.Value == GameState.Play)
                {
                    GameStateManager.SetGameState(GameState.Pause);
                }
            });
    }

    /// <summary>
    /// パネルを開くイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventPanel(CancellationToken ct)
    {
        // ポーズ状態になったら最初のパネルを開く
        GameStateManager.Status
            .TakeUntilDestroy(this)
            .Where(value => value == GameState.Pause)
            .Subscribe(async value =>
            {
                await OpenFirstPanelAsync(ct);
                ChangeIteactive(true);
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

    /// <summary>
    /// パネルを閉じる
    /// </summary>
    /// <param name="ct"></param>
    public override async UniTask ClosePanelAsync(CancellationToken ct)
    {
        await base.ClosePanelAsync(ct);

        // すべてのパネルを閉じたらゲームに戻る
        if(TargetPanel == null)
        {
            ChangeIteactive(false);
            GameStateManager.SetGameState(GameState.Play);
        }
    }
    
    private void ChangeIteactive(bool isInteractive)
    {
        _background.interactable = isInteractive;
        _background.blocksRaycasts = isInteractive;
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