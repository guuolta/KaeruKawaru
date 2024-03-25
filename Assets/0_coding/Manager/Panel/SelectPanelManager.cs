using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class SelectPanelManager : PanelManagerBase<SelectPanelManager>
{
    [Header("セレクトパネル")]
    [SerializeField]
    private SelectPanelPresenter _selectPanel;
    [Header("ステージセレクトパネル")]
    [SerializeField]
    private StageSelectPanelPresenter _stageselectPanel;
    [Header("サウンドパネル")]
    [SerializeField]
    private SelectSoundPanelPresenter _soundPanel;
    [Header("スコアパネル")]
    [SerializeField]
    private ScorePanelPresenter _scorePanel;
    [Header("クレジットパネル")]
    [SerializeField]
    private CreditPanelPresenter _creditPanel;
    protected override void Init()
    {
        base.Init();
        SetFirstPanel();
    }
    private void SetFirstPanel()
    {
        SetFirstPanel(_selectPanel);
    }
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventOpenPanel(Ct);
    }
    /// <summary>
    /// パネルを開くイベントを発行
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventOpenPanel(CancellationToken ct)
    {
        GameStateManager.Status
            .TakeUntilDestroy(this)
            .Where(_ => _ == GameState.Select)
            .DistinctUntilChanged()
            .Subscribe(async _ => {
                await OpenFirstPanelAsync(ct);
            });
    }
    public async UniTask OpenPanelAsync(SelectPanelType type, CancellationToken ct)
    {
        switch(type)
        {
            case SelectPanelType.Slect : await OpenFirstPanelAsync(ct); break;
            case SelectPanelType.StageSelect : await OpenPanelAsync(_stageselectPanel,ct); break;
            case SelectPanelType.Sound : await OpenPanelAsync(_soundPanel,ct); break;
            case SelectPanelType.Score : await OpenPanelAsync(_scorePanel,ct); break;
            case SelectPanelType.Credit : await OpenPanelAsync(_creditPanel,ct); break;
        }
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
    Slect,
    StageSelect,
    Sound,
    Score,
    Credit
}
