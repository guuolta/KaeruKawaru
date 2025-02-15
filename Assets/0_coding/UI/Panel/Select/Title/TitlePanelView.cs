using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UnityEngine;

public class TitlePanelView : PanelViewBase
{
    [Header("クリックテキスト")]
    [SerializeField]
    private TMP_Text _clickText;
    [Header("クリックテキストのアニメーション時間")]
    [Range(0f, 5f)]
    [SerializeField]
    private float _animationTime = 1f;

    private Sequence _sequence;

    protected override void SetEvent()
    {
        base.SetEvent();
        SetTextAnimation(destroyCancellationToken);
    }

    /// <summary>
    /// テキストのアニメーション設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetTextAnimation(CancellationToken ct)
    {
        // タイトルの文字を左から順番に跳ねさせる(無限ループ)
        var tmpAnimator = new DOTweenTMPAnimator(_clickText);
        _sequence = DOTween.Sequence();

        for (int i = 0; i < tmpAnimator.textInfo.characterCount; i++)
        {
            _sequence.Append(
                tmpAnimator.DOOffsetChar(i, new Vector2(0, 10), _animationTime)
                    .SetEase(Ease.InOutFlash, 2)
                );
        }

        _sequence
            .SetLoops(-1, LoopType.Restart) // 無限ループ
            .ToUniTask(cancellationToken: ct)
            .Forget();
    }
}
