using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;

/// <summary>
/// リザルトパネル
/// </summary>
public class ResultUIPresenter : PresenterBase<ResultUIView>
{
    [Header("終了時のパネル")]
    [SerializeField]
    private FinishPanel _finishPanel;

    protected override void Init()
    {
        base.Init();
        View.ChangeInteractive(false);
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventPanel(Ct);
        SetEventButton();
    }

    /// <summary>
    /// パネルのイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventPanel(CancellationToken ct)
    {
        // リザルトステートになったら、UI表示
        GameStateManager.Status
            .TakeUntilDestroy(this)
            .Where(value => value == GameState.Result)
            .DistinctUntilChanged()
            .Subscribe(async _ =>
            {
                // 終了のUI表示
                await _finishPanel.StartAnimationAsync(ct);
                // アニメーションをスキップできるようにする
                SetEventSkipAnimation();

                // リザルトパネル表示
                await ShowAsync(ct);

                // ハイスコアの時は専用のアニメーションを追加
                if (ScoreManager.Instance.HighScoreIndex.Value >= 0)
                {
                    await View.DoNewHighScoreTextAsync(ScoreManager.Instance.HighScoreIndex.Value, ct);
                }

                // ボタンを表示
                await View.ShowButtonAsync(ct);
            });
    }

    /// <summary>
    /// ボタンのイベント設定
    /// </summary>
    private void SetEventButton()
    {
        // 次(前)のレベルに遷移
        View.LevelChangeButton.OnClickCallback += () =>
        {
            AudioManager.Instance.KillSE();
            switch (GameStateManager.StageLevel.Value)
            {
                case Level.Easy:
                    GameSceneManager.LoadScene(SceneType.HardGame);
                    break;
                case Level.Hard:
                    GameSceneManager.LoadScene(SceneType.EasyGame);
                    break;
            }
        };

        // リトライ
        View.RetryButton.OnClickCallback += () =>
        {
            AudioManager.Instance.KillSE();
            GameSceneManager.ReLoadSceneAsync().Forget();
        };

        // タイトルへ
        View.TitleButton.OnClickCallback += () =>
        {
            AudioManager.Instance.KillSE();
            GameSceneManager.LoadScene(SceneType.Title);
        };
    }

    /// <summary>
    /// アニメーションスキップのイベント
    /// </summary>
    private void SetEventSkipAnimation()
    {
        var disposable = new CompositeDisposable();

        //クリックでスキップ
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Select(_ => Input.GetMouseButtonDown(0))
            .SkipWhile(_ => !_)
            .Where(_ => _)
            .Subscribe(_ =>
            {
                View.SkipAnimation();                

                DisposeEvent(disposable);
            }).AddTo(disposable);
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        await base.ShowAsync(ct);
        await SetTextAsync(ct);
    }

    /// <summary>
    /// テキストにスコアを設定
    /// </summary>
    /// <param name="ct"></param>
    private async UniTask SetTextAsync(CancellationToken ct)
    {
        var scoreManager = ScoreManager.Instance;

        View.SetHighScoreText(scoreManager.HighScoreList[0]);
        await View.SetScoreTextAsync(scoreManager.Point.Value, ct);
        await View.SetBounusScoreTextAsync(scoreManager.ClearQuestionCount,
            scoreManager.ComboBonus,
            scoreManager.StepBonus,
            ct);
    }
}
