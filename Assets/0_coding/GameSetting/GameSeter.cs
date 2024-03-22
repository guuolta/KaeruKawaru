using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ゲームの設定
/// </summary>
public class GameSeter : ObjectBase
{
    [Header("fpsの量")]
    [SerializeField]
    private int _fps = 60;

    protected override void Init()
    {
        Application.targetFrameRate = _fps;
        GameStateManager.SetStageLevel(Level.Easy);
    }

    protected async override void SetEvent()
    {
        await UniTask.WaitForSeconds(1f);
        GameStateManager.SetGameState(GameState.Play);
    }

    protected override void Destroy()
    {
       AudioManager.Instance.SaveVolume();
       SaveManager.Save();
    }
}
