using UniRx;

/// <summary>
/// ステートを管理
/// </summary>
public static class GameStateManager
{
    private static ReactiveProperty<GameState> _status = new ReactiveProperty<GameState>(GameState.Play);
    /// <summary>
    /// ステート
    /// </summary>
    public static IReadOnlyReactiveProperty<GameState> Status { get { return _status; } }

    private static ReactiveProperty<Level> _stageLevel = new ReactiveProperty<Level>();
    /// <summary>
    /// ステージのレベル
    /// </summary>
    public static IReadOnlyReactiveProperty<Level> StageLevel => _stageLevel;

    /// <summary>
    /// ポーズ状態を反転する
    /// </summary>
    public static void TogglePauseState()
    {
        if(_status.Value != GameState.Play && _status.Value != GameState.Pause)
        {
            return;
        }

        _status.Value = _status.Value == GameState.Pause ? GameState.Play : GameState.Pause;
    }

    /// <summary>
    /// ステートを設定
    /// </summary>
    /// <param name="state"> ステート </param>
    public static void SetGameState(GameState state)
    {
        if(_status.Value == state)
        {
            return;
        }

        UnityEngine.Debug.Log("ステートを設定 : " + state);
        _status.Value = state;
    }

    /// <summary>
    /// ステージのレベルを設定
    /// </summary>
    /// <param name="level"> レベル </param>
    public static void SetStageLevel(Level level)
    {
        if(_stageLevel.Value == level)
        {
            return;
        }

        UnityEngine.Debug.Log("ステージのレベルを設定 : " + level);
        _stageLevel.Value = level;
    }
}


/// <summary>
/// ステート一覧
/// </summary>
public enum GameState
{
    None,
    Title,
    Start,
    Play,
    Pause,
    Result
}

/// <summary>
/// レベル
/// </summary>
public enum Level
{
    None,
    Easy,
    Hard
}