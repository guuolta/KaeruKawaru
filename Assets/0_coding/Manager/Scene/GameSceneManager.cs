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
    public static async void LoadScene(SceneType type)
    {
        await UniTask.WaitForSeconds(0.1f);
        switch (type)
        {
            case SceneType.Title:
                Debug.Log("タイトル");
                SceneManager.LoadScene(TITLE_SCENE_NAME);
                GameStateManager.SetGameState(GameState.Title);
                break;
            case SceneType.EasyGame:
                Debug.Log("イージー");
                GameStateManager.SetStageLevel(Level.Easy);
                GameStateManager.SetGameState(GameState.Start);
                SceneManager.LoadScene(GAME_SCENE_NAME);
                break;
            case SceneType.HardGame:
                Debug.Log("ハード");
                GameStateManager.SetStageLevel(Level.Hard);
                GameStateManager.SetGameState(GameState.Start);
                SceneManager.LoadScene(GAME_SCENE_NAME);
                break;
        }
    }

    /// <summary>
    /// シーンをリロードする
    /// </summary>
    public static async UniTask ReLoadSceneAsync()
    {
        await UniTask.WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameStateManager.SetGameState(GameState.Start);
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
