using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UniRx;

public class ScoreAnimation : UIBase
{
    private const int ANIMATION_COUNT = 2;

    private TMP_Text scoretext;

    public override void Init()
    {
        base.Init();
        scoretext = GetComponent<TextMeshProUGUI>();
        scoretext.text = "0";
    }
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventScoreAnimation(destroyCancellationToken);
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
                .DOText(score.ToString(), AnimationSec, scrambleMode: ScrambleMode.Numerals)
                .SetEase(Ease.Linear))
            .Join(scoretext
                .DOScale(0.5f, AnimationSec/ ANIMATION_COUNT)
                .SetEase(Ease.OutBack))
            .Append(scoretext.DOScale(1f, AnimationSec/ ANIMATION_COUNT)
                .SetEase(Ease.InBack))
            .ToUniTask(cancellationToken: ct);
    }
}
