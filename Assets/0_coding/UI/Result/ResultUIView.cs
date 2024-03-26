using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UniRx;
using UnityEditor;
using UnityEngine;

public class ResultUIView : ViewBase
{
    private const string EASY_LEVEL_TEXT = "次のレベルへ";
    private const string HARD_LEVEL_TEXT = "前のレベルへ";

    [Header("テキストのアニメーションの時間")]
    [Range(0f, 5f)]
    [SerializeField]
    private float _textAnimationTime = 1f;
    [Header("ハイスコアの色")]
    [SerializeField]
    private VertexGradient _highScoreColor;
    [Header("現在のスコアテキスト")]
    [SerializeField]
    private TMP_Text _scoreText;
    [Header("ハイスコアテキスト")]
    [SerializeField]
    private TMP_Text _highScoreText;
    [Header("お題のクリア数テキスト")]
    [SerializeField]
    private TMP_Text _clearCountText;
    [Header("コンボボーナステキスト")]
    [SerializeField]
    private TMP_Text _comboBonusText;
    [Header("手数ボーナステキスト")]
    [SerializeField]
    private TMP_Text _stepBonusText;
    [Header("レベル変更ボタン")]
    [SerializeField]
    private ButtonBase _levelChangeButton;
    /// <summary>
    /// レベル変更ボタン
    /// </summary>
    public ButtonBase LevelChangeButton => _levelChangeButton;
    [Header("レベル変更のテキスト")]
    [SerializeField]
    private TMP_Text _levelChangeText;
    [Header("リトライボタン")]
    [SerializeField]
    private ButtonBase _retryButton;
    /// <summary>
    /// リトライボタン
    /// </summary>
    public ButtonBase RetryButton => _retryButton;
    [Header("タイトルボタン")]
    [SerializeField]
    private ButtonBase _titleButton;
    /// <summary>
    /// タイトルボタン
    /// </summary>
    public ButtonBase TitleButton => _titleButton;

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventButtonText();
        SetEventScoreAnimation();
    }

    /// <summary>
    /// ボタンのテキストのイベント設定
    /// </summary>
    private void SetEventButtonText()
    {
        GameStateManager.StageLevel
            .TakeUntilDestroy(this)
            .Where(level => level != Level.None)
            .DistinctUntilChanged()
            .Subscribe(level =>
            {
                _levelChangeText.text = level == Level.Easy ? EASY_LEVEL_TEXT : HARD_LEVEL_TEXT;
            });
    }

    /// <summary>
    /// スコアのアニメーション設定
    /// </summary>
    private void SetEventScoreAnimation()
    {
        var disposable = new CompositeDisposable();

        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ =>
            {
                _scoreText.DOComplete();
                DisposeEvent(disposable);
            }).AddTo(disposable);
    }

    /// <summary>
    /// 現在のスコアのテキスト設定
    /// </summary>
    /// <param name="score"> 現在のスコア </param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask SetScoreTextAsync(int score, CancellationToken ct)
    {
        await _scoreText
            .DOText(score.ToString(), _textAnimationTime, scrambleMode: ScrambleMode.Numerals)
            .SetEase(Ease.Linear)
            .ToUniTask(cancellationToken: ct);
    }

    public async UniTask DoHighScoreAnimatinAsync()
    {
        _highScoreText.colorGradient = _highScoreColor;
        AudioManager.Instance.PlayOneShotSE(SEType.Fanfare);
    }

    /// <summary>
    /// スコア設定
    /// </summary>
    /// <param name="highScore"> これまでのハイスコア </param>
    /// <param name="clearCount"> お題のクリア数 </param>
    /// <param name="comboBonus"> コンボボーナス </param>
    /// <param name="stepBonus"> 手数ボーナス </param>
    public void SetScoreText(int highScore, int clearCount, int comboBonus, int stepBonus)
    {
        _highScoreText.text = highScore.ToString();
        _clearCountText.text = clearCount.ToString();
        _stepBonusText.text = stepBonus.ToString();
        _comboBonusText.text = comboBonus.ToString();
    }
    
    public override async UniTask ShowAsync(CancellationToken ct)
    {
        await ShowAsync(CanvasGroup, ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        await HideAsync(CanvasGroup, ct);
    }

}