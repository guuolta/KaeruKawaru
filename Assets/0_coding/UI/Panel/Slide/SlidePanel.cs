using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// ページめくりができるパネル
/// </summary>
public class SlidePanel : UIBase
{
    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="pos">初期位置</param>
    public void SetInit(Vector2 pos)
    {
        RectTransform.anchoredPosition = pos;
        ChangeInteractive(false);
    }
    
    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="posX">表示時の位置</param>
    /// <param name="ct"></param>
    public async UniTask ShowAsync(float posX,CancellationToken ct)
    {
        ChangeInteractive(true);
        RectTransform.DOComplete();

        // 横移動で表示
        await RectTransform
            .DOAnchorPosX(posX,AnimationSec)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken : ct);
    }
    
    /// <summary>
    /// 非表示
    /// </summary>
    /// <param name="posX">非表示時の位置</param>
    /// <param name="ct"></param>
    public async UniTask HideAsync(float posX,CancellationToken ct)
    {
        RectTransform.DOComplete();

        // 横移動で非表示
        await RectTransform
            .DOAnchorPosX(posX,AnimationSec)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken : ct);

        ChangeInteractive(false);
    }
}
