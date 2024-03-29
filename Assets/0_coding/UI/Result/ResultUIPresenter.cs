using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;

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
            .Subscribe(async _ =>
            {
                await ShowAsync(ct);

                if (ScoreManager.Instance.HighScoreIndex.Value >= 0)
                {
                    Debug.Log(ScoreManager.Instance.HighScoreIndex.Value);
                    await View.DoNewHighScoreTextAsync(ScoreManager.Instance.HighScoreIndex.Value, ct);
                }

                await View.ShowButtonAsync(ct);
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
        await base.ShowAsync(ct);
        await SetTextAsync(ct);
    }

    /// <summary>
    /// テキストにスコアを設定
    /// </summary>
    /// <param name="ct"></param>
    private async UniTask SetTextAsync(CancellationToken ct)
    {
        var _scoreManager = ScoreManager.Instance;

        View.SetHighScoreText(_scoreManager.HighScoreList[0]);
        await View.SetScoreTextAsync(_scoreManager.Point.Value, ct);
        await View.SetBounusScoreTextAsync(_scoreManager.ClearQuestionCount,
            _scoreManager.ComboBonus,
            _scoreManager.StepBonus,
            ct);
    }
}
