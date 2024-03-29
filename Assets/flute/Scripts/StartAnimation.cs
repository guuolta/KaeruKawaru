using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UniRx;
using UnityEngine;

public class StartAnimation : UIBase
{
    private const int ANIMATION_COUNT = 4;

    [Header("開始前の文章")]
    [SerializeField]
    private string _readyText = "よーい";
    [Header("開始の文章")]
    [SerializeField]
    private string _startText = "どん!!";
    [Header("開始時のSE")]
    [SerializeField]
    private AudioClip _startSE;

    private TextMeshProUGUI _readytext, _starttext;
    private CompositeDisposable _disposable = new CompositeDisposable();

    protected override void Init()
    {
        base.Init();

        _readytext = Transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _starttext = Transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _starttext.text = _startText;
        _starttext.transform.localScale = Vector3.zero;
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventStartAnimatin(Ct);
    }

    private void SetEventStartAnimatin(CancellationToken ct)
    {
        GameStateManager.Status
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Where(value => value == GameState.Start)
            .Subscribe(async _ =>
            {
                await DoStartAnimationAsync(ct);
                GameStateManager.SetGameState(GameState.Play);
                DisposeEvent(_disposable);
            }).AddTo(_disposable);
    }

    public async UniTask DoStartAnimationAsync(CancellationToken ct)
    {
        var sequence = DOTween.Sequence();

        await sequence.Append(_readytext
                .DOText(_readyText, AnimationTime/ ANIMATION_COUNT)
                .SetEase(Ease.Linear))
            .Append(_readytext
                .DOFade(0, AnimationTime / ANIMATION_COUNT)
                .SetEase(Ease.Linear))
            .AppendCallback(() => AudioManager.Instance.PlayOneShotSE(_startSE))
            .Append(_starttext
                .DOScale(1.2f, AnimationTime / ANIMATION_COUNT)
                .SetEase(Ease.OutExpo))
            .Append(_starttext
                .DOFade(0, AnimationTime / ANIMATION_COUNT))
            .ToUniTask(cancellationToken: ct);
        ChangeInteractive(false);
    }


    protected override void Destroy()
    {
        base.Destroy();
        DisposeEvent(_disposable);
    }
}