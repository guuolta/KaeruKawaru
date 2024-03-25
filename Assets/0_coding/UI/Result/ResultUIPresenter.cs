using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using unityroom.Api;

public class ResultUIPresenter : PresenterBase<ResultUIView>
{
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
        GameStateManager.Status
            .TakeUntilDestroy(this)
            .Where(value => value == GameState.Result)
            .DistinctUntilChanged()
            .Subscribe(_ =>
            {
                ShowAsync(ct).Forget();
            });
    }

    /// <summary>
    /// ボタンのイベント設定
    /// </summary>
    private void SetEventButton()
    {
        View.LevelChangeButton.OnClickCallback += () =>
        {
            switch(GameStateManager.StageLevel.Value)
            {
                case Level.Easy:
                    GameSceneManager.LoadScene(SceneType.HardGame);
                    break;
                case Level.Hard:
                    GameSceneManager.LoadScene(SceneType.EasyGame);
                    break;
            }
        };

        View.RetryButton.OnClickCallback += () =>
        {
            GameSceneManager.ReLoadSceneAsync().Forget();
        };

        View.TitleButton.OnClickCallback += () =>
        {
            GameSceneManager.LoadScene(SceneType.Title);
        };
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        SetText(ct);
        UnityroomApiClient.Instance.SendScore(1, ScoreManager.Instance.Point.Value, ScoreboardWriteMode.HighScoreDesc);
        await base.ShowAsync(ct);
    }

    /// <summary>
    /// テキストにスコアを設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetText(CancellationToken ct)
    {
        var _scoreManager = ScoreManager.Instance;
        
        View.SetScoreTextAsync(_scoreManager.Point.Value, ct).Forget();
        View.SetScoreText(_scoreManager.HighScoreList[0],
            _scoreManager.ClearQuestionCount,
            _scoreManager.ComboBonus,
            _scoreManager.StepBonus);
    }
}
