using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// 遊び方
/// </summary>
public class HowToPlayPanelPresenter : SelectPanelPresenterBase<HowToPlayPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetButton(Ct);
    }
    private void SetButton(CancellationToken ct)
    {
        // ページを戻す
        View.LeftButton.OnClickCallback += () => {
            View.SlideLeftAsync(ct).Forget();
        };
        
        // ページを進める
        View.RightButton.OnClickCallback += () => {
            View.SlideRightAsync(ct).Forget();
        };
    }
}
