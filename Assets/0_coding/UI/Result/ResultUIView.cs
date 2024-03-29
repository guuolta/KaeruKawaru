using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
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
    [Header("現在のスコアテキスト")]
    [SerializeField]
    private TMP_Text _scoreText;
    [Header("ハイスコアを更新したときのスコアテキスト")]
    [SerializeField]
    private TMP_Text _newHighScoreText;
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

    [Header("ボタングループ")]
    [SerializeField]
    private RectTransform _buttonGroup;
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

    [Header("ハイスコアのアニメーション時間")]
    [Range(0f, 5f)]
    [SerializeField]
    private float _highScoreAnimationTime = 0.2f;
    [Header("ハイスコアの色(昇順)")]
    [SerializeField]
    private List<VertexGradient> _highScoreColorList = new List<VertexGradient>();
    [Header("ハイスコアの吹き出し")]
    [SerializeField]
    private HighScoreBubble _highScoreBubble;

    [Header("ボーナススコアのアニメーション時間")]
    [Range(0f, 5f)]
    [SerializeField]
    private float _bonusAnimationTime = 0.3f;

    private bool _doCompleate = false;
    private float _iniPosY;
    private float _targetPosY;
    private Sequence _highScoreTextSequence;

    protected override void Init()
    {
        base.Init();
        _targetPosY = _buttonGroup.anchoredPosition.y;
        _iniPosY = _targetPosY - _buttonGroup.rect.size.y;
        _buttonGroup.anchoredPosition = new Vector2(_buttonGroup.anchoredPosition.x, _iniPosY);
        ChangeButtonInteractive(false);

        _highScoreTextSequence = DOTween.Sequence();
        _newHighScoreText.transform.localScale = Vector3.zero;
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventButtonText();
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
    public void SetEventScoreAnimation()
    {
        var disposable = new CompositeDisposable();

        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Select(_ => Input.GetMouseButtonDown(0))
            .SkipWhile(_ => !_)
            .Where(_ => _)
            .Subscribe(_ =>
            {
                _doCompleate = true;
                _scoreText.DOComplete();
                _clearCountText.DOComplete();
                _comboBonusText.DOComplete();
                _stepBonusText.DOComplete();

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
        await DOScoreText(_scoreText, score.ToString(), _textAnimationTime, ct);
    }

    /// <summary>
    /// ハイスコアのテキスト設定
    /// </summary>
    /// <param name="index"> 順位 </param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask DoNewHighScoreTextAsync(int index, CancellationToken ct)
    {
        _newHighScoreText.text = _scoreText.text;
        _newHighScoreText.colorGradient = _highScoreColorList[index];

        AudioManager.Instance.PlayOneShotSE(SEType.Fanfare);
        await _highScoreTextSequence
            .Append(_scoreText
                .DOScale(0, _highScoreAnimationTime / 2)
                .SetEase(Ease.OutSine))
            .Append(_newHighScoreText
                .DOScale(1, _highScoreAnimationTime / 2)
                .SetEase(Ease.InSine))
            .AppendCallback(async () =>
            {
                if (!_doCompleate)
                    await _highScoreBubble.ShowAsync(ct);
            })
            .OnUpdate(() =>
            {
                if (_doCompleate)
                {
                    _highScoreTextSequence.Complete();
                    _highScoreBubble.ShowAsync(ct).Forget();
                    _highScoreBubble.DOComplete();
                }
            })
            .ToUniTask(cancellationToken: ct);
    }

    /// <summary>
    /// ハイスコア設定
    /// </summary>
    /// <param name="highScore"> これまでのハイスコア </param>
    public void SetHighScoreText(int highScore)
    {
        _highScoreText.text = highScore.ToString();
    }

    /// <summary>
    /// ボーナススコア設定
    /// </summary>
    /// <param name="clearCount"> お題のクリア数 </param>
    /// <param name="comboBonus"> コンボボーナス </param>
    /// <param name="stepBonus"> 手数ボーナス </param>
    public async UniTask SetBounusScoreTextAsync(int clearCount, int comboBonus, int stepBonus, CancellationToken ct)
    {
        await DOScoreText(_clearCountText, clearCount.ToString(), _bonusAnimationTime, ct);
        await DOScoreText(_comboBonusText, comboBonus.ToString(), _bonusAnimationTime, ct);
        await DOScoreText(_stepBonusText, stepBonus.ToString(), _bonusAnimationTime, ct);
    }

    /// <summary>
    /// スコアテキストのアニメーションを実行
    /// </summary>
    /// <param name="text"> 対象のテキスト </param>
    /// <param name="content"> 表示する内容 </param>
    /// <param name="animationTime"> アニメーションの時間 </param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask DOScoreText(TMP_Text text, string content, float animationTime, CancellationToken ct)
    {
        if(_doCompleate)
        {
            text.text = content;
            return;
        }

        float timeValue = 0;
        float interval = animationTime / content.Length * 0.8f;

        await text
            .DOText(content, animationTime, scrambleMode: ScrambleMode.Numerals)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                timeValue += Time.deltaTime;
                if(timeValue < interval)
                {
                    return;
                }

                AudioManager.Instance.PlayOneShotSE(SEType.Text);
                timeValue = 0;
            })
            .ToUniTask(cancellationToken: ct);
    }

    /// <summary>
    /// ボタンを表示
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask ShowButtonAsync(CancellationToken ct)
    {
        _buttonGroup.DOComplete();

        await _buttonGroup
                .DOAnchorPosY(_targetPosY, AnimationTime)
                .SetEase(Ease.InSine)
                .ToUniTask(cancellationToken: ct);
        ChangeButtonInteractive(true);
    }

    /// <summary>
    /// ボタンを押せるようにするか設定
    /// </summary>
    /// <param name="isInteractive"> ボタンを押せるようにするか </param>
    private void ChangeButtonInteractive(bool isInteractive)
    {
        _levelChangeButton.ChangeInteractive(isInteractive);
        _retryButton.ChangeInteractive(isInteractive);
        _titleButton.ChangeInteractive(isInteractive);
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