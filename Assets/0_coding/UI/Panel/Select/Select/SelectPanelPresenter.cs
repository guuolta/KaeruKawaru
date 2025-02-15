using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

public class SelectPanelPresenter : PanelPresenterBase<SelectPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventButton(destroyCancellationToken);
    }
    
    /// <summary>
    /// ボタンの設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventButton(CancellationToken ct)
    {
        // ステージセレクト
        View.StageSelectButton.OnClickEvent
            .Subscribe(_ => 
            {
                SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.StageSelect,ct).Forget();
            });
        
        // 音量調節
        View.SoundButton.OnClickEvent
            .Subscribe(_ => 
            {
                SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Sound,ct).Forget();
            });
        
        // ランキング
        View.RankButton.OnClickEvent
            .Subscribe(_ => {
                SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Score,ct).Forget();
            });
        
        // クレジット
        View.CreditButton.OnClickEvent
            .Subscribe(_ => {
                SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Credit,ct).Forget();
            });
        
        // 遊び方
        View.HowPlayButton.OnClickEvent
            .Subscribe(_ => {
                SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.HowToPlay,ct).Forget();
            });
        
        // 閉じるボタン
        View.CloseButton.OnClickEvent
            .Subscribe(_ => {
                SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Title,ct).Forget();
            });
    }
}
