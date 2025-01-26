using System.Threading;
using Cysharp.Threading.Tasks;

public class SelectPanelPresenter : PanelPresenterBase<SelectPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventButton(Ct);
    }
    
    /// <summary>
    /// ボタンの設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventButton(CancellationToken ct)
    {
        // ステージセレクト
        View.StageSelectButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.StageSelect,ct).Forget();
        };
        
        // 音量調節
        View.SoundButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Sound,ct).Forget();
        };
        
        // ランキング
        View.RankButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Score,ct).Forget();
        };
        
        // クレジット
        View.CreditButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Credit,ct).Forget();
        };
        
        // 遊び方
        View.HowPlayButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.HowToPlay,ct).Forget();
        };
        
        // 閉じるボタン
        View.CloseButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Title,ct).Forget();
        };
    }
}
