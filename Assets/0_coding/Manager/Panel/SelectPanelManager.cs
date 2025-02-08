using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SelectPanelManager : PanelManagerBase<SelectPanelManager>
{
    [Header("タイトルパネル")]
    [SerializeField]
    private TitlePanelPresenter _titlePanel;
    [Header("セレクトパネル")]
    [SerializeField]
    private SelectPanelPresenter _selectPanel;
    [Header("ステージセレクトパネル")]
    [SerializeField]
    private StageSelectPanelPresenter _stageSelectPanel;
    [Header("サウンドパネル")]
    [SerializeField]
    private SelectSoundPanelPresenter _soundPanel;
    [Header("スコアパネル")]
    [SerializeField]
    private ScorePanelPresenter _scorePanel;
    [Header("クレジットパネル")]
    [SerializeField]
    private CreditPanelPresenter _creditPanel;
    [Header("遊び方パネル")]
    [SerializeField]
    private HowToPlayPanelPresenter _howToPlayPanel;
    protected override void Init()
    {
        base.Init();
        SetFirstPanel();
        OpenFirstPanelAsync(Ct).Forget();
    }
    
    /// <summary>
    /// 最初に開くパネルを設定
    /// </summary>
    private void SetFirstPanel()
    {
        SetFirstPanel(_titlePanel);
    }
    
    public override async UniTask OpenFirstPanelAsync(CancellationToken ct)
    {
        await base.OpenFirstPanelAsync(ct);
        await TitleManager.Instance.TargetTitleAsync(ct);
    }
    public async UniTask OpenPanelAsync(SelectPanelType type, CancellationToken ct)
    {
        IPresenter panel = null;

        // 開くパネルをえらぶ(タイトルはその場で開いて、終了)
        switch(type)
        {
            case SelectPanelType.Title: await OpenFirstPanelAsync(ct); return;
            case SelectPanelType.Slect : panel = _selectPanel;  break;
            case SelectPanelType.StageSelect : panel = _stageSelectPanel; break;
            case SelectPanelType.Sound : panel = _soundPanel; break;
            case SelectPanelType.Score : panel = _scorePanel; break;
            case SelectPanelType.Credit : panel = _creditPanel; break;
            case SelectPanelType.HowToPlay: panel = _howToPlayPanel; break;
        }

        await OpenPanelAsync(panel, ct);
    }
    
    public override async UniTask ClosePanelAsync(CancellationToken ct)
    {
        await base.ClosePanelAsync(ct);
        // すべてのパネルを閉じたらタイトル状態にする
        if(TargetPanel == null)
        {
            GameStateManager.SetGameState(GameState.Title);
        }
    }
}

/// <summary>
/// セレクトパネル
/// </summary>
public enum SelectPanelType
{
    None,
    Title,
    Slect,
    StageSelect,
    Sound,
    Score,
    Credit,
    HowToPlay
}
