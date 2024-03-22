using UnityEngine.SceneManagement;

public static class GameSceneManager
{
    private const string TITLE_SCENE_NAME = "Title";
    private const string GAME_SCENE_NAME = "Main";

    /// <summary>
    /// シーンをロードする
    /// </summary>
    /// <param name="type"> シーンの種類 </param>
    public static void LoadScene(SceneType type)
    {
        switch(type)
        {
            case SceneType.Title:
                SceneManager.LoadScene(TITLE_SCENE_NAME);
                break;
            case SceneType.EasyGame:
                GameStateManager.SetStageLevel(Level.Easy);
                SceneManager.LoadScene(GAME_SCENE_NAME);
                break;
            case SceneType.HardGame:
                GameStateManager.SetStageLevel(Level.Hard);
                SceneManager.LoadScene(GAME_SCENE_NAME);
                break;
        }
    }

    /// <summary>
    /// シーンをリロードする
    /// </summary>
    public static void ReLoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

/// <summary>
/// シーンの種類
/// </summary>
public enum SceneType
{
    Title,
    EasyGame,
    HardGame
}
