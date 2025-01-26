using System.Threading;

/// <summary>
/// タイトルのパネル
/// </summary>
public class TitlePanelPresenter : PanelPresenterBase<TitlePanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventClick(Ct);
    }

    /// <summary>
    /// UIをクリックしたときのイベント
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventClick(CancellationToken ct)
    {
        View.OnClickCallback += async () =>
        {
            //SEを鳴らす
            AudioManager.Instance.PlayOneShotSE(SEType.Posi);
            // カメラを移動
            await TitleManager.Instance.TargetSelectAsync(ct);
            // メニューセレクトを開く
            await SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Slect, ct);
        };
    }
}
