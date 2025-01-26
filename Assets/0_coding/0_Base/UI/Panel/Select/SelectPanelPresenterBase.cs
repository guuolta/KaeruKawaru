using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// セレクト画面のパネルプレゼンター
/// </summary>
/// <typeparam name="TView"></typeparam>
public class SelectPanelPresenterBase<TView> : PanelPresenterBase<TView>
    where TView : SelectPanelViewBase
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetCloseButton(Ct);
    }

    /// <summary>
    /// 閉じるボタンの設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetCloseButton(CancellationToken ct)
    {
        // 閉じるボタンが押されたらメニューセレクトパネルを表示
        View.CloseButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Slect,ct).Forget();
        };
    }
}
