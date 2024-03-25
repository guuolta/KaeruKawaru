using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationPartBase : UIBase
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        if(Transform == null || CanvasGroup == null)
        {
            return;
        }

        Transform
            .DOScale(0.8f, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
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

        Transform.DOScale(1f, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
        CanvasGroup.DOFade(1f, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Transform
            .DOScale(1.05f, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Transform
            .DOScale(1f, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }
}
