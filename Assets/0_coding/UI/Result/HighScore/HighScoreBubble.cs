using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

/// <summary>
/// ハイスコアの吹き出し
/// </summary>
public class HighScoreBubble : UIBase
{
    protected override void Init()
    {
        base.Init();
        
        //非表示
        RectTransform.localScale = Vector3.zero;
        ChangeInteractive(false);
    }

    /// <summary>
    /// UI表示
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask ShowAsync(CancellationToken ct)
    {
        ChangeInteractive(true);

        // 拡大表示
        RectTransform.DOComplete();
        await RectTransform
            .DOScale(Vector3.one, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: ct);
    }
}
