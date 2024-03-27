using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

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
    private void SetFirstPanel()
    {
        SetFirstPanel(_titlePanel);
    }

    public async UniTask OpenPanelAsync(SelectPanelType type, CancellationToken ct)
    {
        IPresenter panel = null;

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
        if(TargetPanel == null)
        {
            GameStateManager.SetGameState(GameState.Title);
        }
    }
}

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
