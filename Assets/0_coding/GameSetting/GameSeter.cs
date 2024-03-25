using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ゲームの設定
/// </summary>
public class GameSeter : DontDestroySingletonObject<GameSeter>
{
    [Header("fpsの量")]
    [SerializeField]
    private int _fps = 60;

    protected override void Init()
    {
        Application.targetFrameRate = _fps;
        GameStateManager.SetStageLevel(Level.Easy);
    }

    protected override void Destroy()
    {
       AudioManager.Instance.SaveVolume();
       SaveManager.Save();
    }
}
