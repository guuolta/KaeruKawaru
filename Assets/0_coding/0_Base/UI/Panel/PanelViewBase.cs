using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

/// <summary>
/// パネルのビューのベース
/// </summary>
public class PanelViewBase : ViewBase
{
    public virtual async UniTask ShowAsync(CancellationToken ct)
    {
        if(Transform.localScale != Vector3.zero)
        {
            return;
        }

        // 拡大表示
        await DoScaleAsync(1f, Ease.InSine);
    }

    public virtual void Show()
    {
        Transform.localScale = Vector2.one;
    }
    
    public virtual async UniTask HideAsync(CancellationToken ct)
    {
        if(Transform.localScale == Vector3.zero)
        {
            return;
        }

        // 縮小して非表示
        await DoScaleAsync(0f, Ease.OutSine);
    }
    
    public virtual void Hide()
    {
        Transform.localScale = Vector2.zero;
    }
}
