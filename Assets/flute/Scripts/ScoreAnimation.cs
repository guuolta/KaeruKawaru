using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UniRx;

public class ScoreAnimation : UIBase
{
    private const int ANIMATION_COUNT = 2;

    private TMP_Text scoretext;

    protected override void Init()
    {
        base.Init();
        scoretext = GetComponent<TextMeshProUGUI>();
        scoretext.text = "0";
    }
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventScoreAnimation(Ct);
    }

    private void SetEventScoreAnimation(CancellationToken ct)
    {
        ScoreManager.Instance.Point
            .SkipWhile(value => value <= 0)
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                await DoScoreAnimationAsync(value, ct);
            });
    }

    private async UniTask DoScoreAnimationAsync(int score, CancellationToken ct)
    {
        scoretext.DOComplete();

        var sequence = DOTween.Sequence();
        await sequence
            .Append(scoretext
                .DOText(score.ToString(), AnimationTime, scrambleMode: ScrambleMode.Numerals)
                .SetEase(Ease.Linear))
            .Join(scoretext
                .DOScale(0.5f, AnimationTime/ ANIMATION_COUNT)
                .SetEase(Ease.OutBack))
            .Append(scoretext.DOScale(1f, AnimationTime/ ANIMATION_COUNT)
                .SetEase(Ease.InBack))
            .ToUniTask(cancellationToken: ct);
    }
}
