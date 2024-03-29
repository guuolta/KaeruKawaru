using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SlidePanel : UIBase
{
    public void SetInipos(Vector2 pos)
    {
        RectTransform.anchoredPosition = pos;
        ChangeInteractive(false);
    }
    public async UniTask ShowAsync(float posX,CancellationToken ct)
    {
        ChangeInteractive(true);
        RectTransform.DOComplete();

        await RectTransform
            .DOAnchorPosX(posX,AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken : ct);
        
    }
    public async UniTask HideAsync(float posX,CancellationToken ct)
    {
        RectTransform.DOComplete();

        await RectTransform
            .DOAnchorPosX(posX,AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken : ct);

        ChangeInteractive(false);
    }
}
