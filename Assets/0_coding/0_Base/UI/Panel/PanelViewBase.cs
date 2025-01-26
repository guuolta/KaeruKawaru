using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

/// <summary>
/// パネルのビューのベース
/// </summary>
public class PanelViewBase : ViewBase
{
    protected override void Init()
    {
        Transform.localScale = Vector3.zero;
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        if(Transform.localScale != Vector3.zero)
        {
            return;
        }

        // アニメーションをしていたら停止
        Transform.DOComplete();
        // 拡大表示
        await Transform
            .DOScale(Vector2.one, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: ct);
    }
    
    public override async UniTask HideAsync(CancellationToken ct)
    {
        if(Transform.localScale == Vector3.zero)
        {
            return;
        }

        // 再生中のアニメーションを停止
        Transform.DOComplete();
        // 縮小して非表示
        await Transform
            .DOScale(Vector2.zero, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: ct);
    }
}
