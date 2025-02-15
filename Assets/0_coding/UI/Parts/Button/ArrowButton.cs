using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine.EventSystems;

/// <summary>
/// ページめくりに使うボタン
/// </summary>
public class ArrowButton : ButtonBase
{
    private bool _isHide;
    protected override void SetEventDobleClickPrevention()
    {
        OnClickEvent
            .Subscribe(async _ =>
            {
                if(_isHide)
                {
                    return;
                }

                ChangeInteractive(false);
                await UniTask.WaitForSeconds(0.1f, cancellationToken: destroyCancellationToken);

                if (destroyCancellationToken.IsCancellationRequested || _isHide) return;
                ChangeInteractive(true);
            });
    }
    
    /// <summary>
    /// 非表示か設定する
    /// </summary>
    /// <param name="ishide">非表示か</param>
    public void SetIsHide(bool ishide)
    {
        _isHide =ishide;
        ChangeInteractive(ishide);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (Transform == null)
        {
            return;
        }

        // 縮小
        DoScaleAsync(0.8f, Ease.InSine).Forget();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (Transform == null)
        {
            return;
        }

        // 元の大きさにする
        DoScaleAsync(1f, Ease.OutSine).Forget();
    }
}
