using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishPanel : UIBase
{
    private const int ANIMATION_COUNT = 4;

    [Header("終了時のSE")]
    [SerializeField]
    private AudioClip _finishSE;
    [Header("終了のテキスト")]
    [SerializeField]
    private TMP_Text _finishtext;
    [Header("テキストの初期位置")]
    [SerializeField]
    private Vector2 _iniPosition;
    [Header("テキストの表示位置(X)")]
    [SerializeField]
    private float _showPositionX;
    [Header("テキストの消す位置(X)")]
    [SerializeField]
    private float _hidePositionX;

    protected override void Init()
    {
        base.Init();
        _finishtext.rectTransform.anchoredPosition = _iniPosition;
        Hide(CanvasGroup);
        ChangeInteractive(false);
    }

    public async UniTask StartAnimation(CancellationToken ct)
    {
        ChangeInteractive(true);
        AudioManager.Instance.PlayOneShotSE(_finishSE);

        var sequence = DOTween.Sequence();
        await sequence
            .Append(CanvasGroup
                .DOFade(1, AnimationTime/ ANIMATION_COUNT))
                .SetEase(Ease.InSine)
            .Append(_finishtext.rectTransform
                .DOAnchorPosX(_showPositionX, AnimationTime/ ANIMATION_COUNT)
                .SetEase(Ease.InSine))
            .AppendInterval(AnimationTime / ANIMATION_COUNT)
            .Append(_finishtext.rectTransform
                .DOAnchorPosX(_hidePositionX, AnimationTime / ANIMATION_COUNT)
                .SetEase(Ease.OutSine))
            .ToUniTask(cancellationToken: ct);
    }
}
