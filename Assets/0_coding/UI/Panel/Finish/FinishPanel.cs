using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UnityEngine;

/// <summary>
/// ゲーム終了時のパネル
/// </summary>
public class FinishPanel : UIBase
{
    /// <summary>
    /// アニメーションする数
    /// </summary>
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
        // 初期位置に移動
        _finishtext.rectTransform.anchoredPosition = _iniPosition;
        
        //非表示
        Hide(CanvasGroup);
        ChangeInteractive(false);
    }

    // アニメーションする
    public async UniTask StartAnimationAsync(CancellationToken ct)
    {
        // 他のUIを触れなくする
        ChangeInteractive(true);
        // SEを鳴らす
        AudioManager.Instance.PlayOneShotSE(_finishSE);

        var sequence = DOTween.Sequence();
        await sequence
            // UI表示
            .Append(CanvasGroup 
                .DOFade(1, AnimationTime/ ANIMATION_COUNT))
                .SetEase(Ease.InSine)
            // 文字を右から左に表示
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
