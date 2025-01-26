using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.EventSystems;

/// <summary>
/// ページめくりに使うボタン
/// </summary>
public class ArrowButton : ButtonBase
{
    private bool _isHide;
    protected override void SetEventDobleClickPrevention()
    {
        OnClickCallback += async () =>
        {
            if(_isHide)
            {
                return;
            }

            ChangeInteractive(false);
            await UniTask.WaitForSeconds(0.1f, cancellationToken: Ct);

            if (Ct.IsCancellationRequested || _isHide) return;
            ChangeInteractive(true);
        };
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
        Transform
            .DOScale(0.8f, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (Transform == null)
        {
            return;
        }

        // 元の大きさにする
        Transform.DOScale(1f, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }
}
