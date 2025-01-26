using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.EventSystems;

/// <summary>
/// 基本的なUIのパーツのアニメーションを共通化
/// </summary>
public class AnimationPartBase : UIBase
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        if(Transform == null || CanvasGroup == null)
        {
            return;
        }

        // 縮小
        Transform
            .DOScale(0.8f, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
        // 半透明にする
        CanvasGroup
            .DOFade(0.8f, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (Transform == null || CanvasGroup == null)
        {
            return;
        }

        // 元の大きさに戻す
        Transform.DOScale(1f, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
        // 元の透明度にする
        CanvasGroup.DOFade(1f, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        // 拡大
        Transform
            .DOScale(1.05f, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        // 元の大きさにする
        Transform
            .DOScale(1f, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }
}
