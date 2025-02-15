using System.Threading;
using UnityEngine.EventSystems;

/// <summary>
/// タイトルのパネル
/// </summary>
public class TitlePanelPresenter : PanelPresenterBase<TitlePanelView>
    ,IPointerClickHandler
{
    public async void OnPointerClick(PointerEventData eventData)
    {
        //SEを鳴らす
        AudioManager.Instance.PlayOneShotSE(SEType.Posi);
        // カメラを移動
        await TitleManager.Instance.TargetSelectAsync(destroyCancellationToken);
        // メニューセレクトを開く
        await SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Slect, destroyCancellationToken);
    }
}
