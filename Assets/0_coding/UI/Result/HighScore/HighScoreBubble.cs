using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class HighScoreBubble : UIBase
{
    protected override void Init()
    {
        base.Init();
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

        RectTransform.DOComplete();
        await RectTransform
            .DOScale(Vector3.one, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: ct);
    }
}
