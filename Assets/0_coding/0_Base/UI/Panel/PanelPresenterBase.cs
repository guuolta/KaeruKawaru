using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// パネルのプレゼンター
/// </summary>
/// <typeparam name="TView"> パネルビュー </typeparam>
public class PanelPresenterBase<TView> : PresenterBase<TView>, IPanelPresenter
    where TView : PanelViewBase
{
    public override void Init()
    {
        base.Init();
        Hide();
    }
    
    /// <summary>
    /// UIをアニメーションで表示
    /// </summary>
    /// <param name="ct"></param>
    public virtual async UniTask ShowAsync(CancellationToken ct)
    {
        await View.ShowAsync(ct);
        View.ChangeInteractive(true);
    }

    /// <summary>
    /// UIを即座に表示
    /// </summary>
    public void Show()
    {
        View.Show();
        View.ChangeInteractive(true);
    }

    /// <summary>
    /// UIをアニメーションで非表示
    /// </summary>
    /// <param name="ct"></param>
    public virtual async UniTask HideAsync(CancellationToken ct)
    {
        View.ChangeInteractive(false);
        await View.HideAsync(ct);
    }
    
    /// <summary>
    /// UIを即座に非表示
    /// </summary>
    public void Hide()
    {
        View.ChangeInteractive(false);
        View.Hide();
    }
}
