using Cysharp.Threading.Tasks;
using System.Threading;

public class HowToPlayPanelPresenter : SelectPanelPresenterBase<HowToPlayPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetButton(Ct);
    }
    private void SetButton(CancellationToken ct)
    {
        View.LeftButton.OnClickCallback += () => {
            View.SlideLeftAsync(ct).Forget();
        };
        View.RightButton.OnClickCallback += () => {
            View.SlideRightAsync(ct).Forget();
        };
    }
}
