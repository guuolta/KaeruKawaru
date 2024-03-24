using UnityEngine.SceneManagement;
using UnityEngine;
using Cysharp.Threading.Tasks;

public static class GameSceneManager
{
    private const string TITLE_SCENE_NAME = "Title";
    private const string GAME_SCENE_NAME = "MainGame";

    /// <summary>
    /// シーンをロードする
    /// </summary>
    /// <param name="type"> シーンの種類 </param>
    public static void LoadScene(SceneType type)
    {
        switch(type)
        {
            case SceneType.Title:
                GameStateManager.SetGameState(GameState.Load);
                Debug.Log("タイトル");
                SceneManager.LoadScene(TITLE_SCENE_NAME);
                break;
            case SceneType.EasyGame:
                Debug.Log("イージー");
                GameStateManager.SetGameState(GameState.Load);
                GameStateManager.SetStageLevel(Level.Easy);
                SceneManager.LoadScene(GAME_SCENE_NAME);
                break;
            case SceneType.HardGame:
                Debug.Log("ハード");
                GameStateManager.SetGameState(GameState.Load);
                GameStateManager.SetStageLevel(Level.Hard);
                SceneManager.LoadScene(GAME_SCENE_NAME);
                break;
        }
    }

    /// <summary>
    /// シーンをリロードする
    /// </summary>
    public static async UniTask ReLoadSceneAsync()
    {
        GameStateManager.SetGameState(GameState.Load);
        await UniTask.WaitForSeconds(0.1f);
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
