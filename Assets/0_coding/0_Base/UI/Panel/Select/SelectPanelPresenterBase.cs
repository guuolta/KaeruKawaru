using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

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
        SetCloseButton(destroyCancellationToken);
    }

    /// <summary>
    /// 閉じるボタンの設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetCloseButton(CancellationToken ct)
    {
        // 閉じるボタンが押されたらメニューセレクトパネルを表示
        View.CloseButton.OnClickEvent
            .Subscribe(_ => {
                SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Slect,ct).Forget();
            });
    }
}
