using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

/// <summary>
/// 遊び方
/// </summary>
public class HowToPlayPanelPresenter : SelectPanelPresenterBase<HowToPlayPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetButton(destroyCancellationToken);
    }
    private void SetButton(CancellationToken ct)
    {
        // ページを戻す
        View.LeftButton.OnClickEvent
            .Subscribe(_ => {
                View.SlideLeftAsync(ct).Forget();
            });
        
        // ページを進める
        View.RightButton.OnClickEvent
            .Subscribe(_ => {
                View.SlideRightAsync(ct).Forget();
            });
    }
}
