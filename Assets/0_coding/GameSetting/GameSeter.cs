using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ゲームの設定
/// </summary>
public class GameSeter : DontDestroySingletonObject<GameSeter>
{
    [Header("セーブデータをリセットするか")]
    [SerializeField]
    private bool _isResetSaveData = false;
    [Header("fpsの量")]
    [SerializeField]
    private int _fps = 60;

    protected override void Init()
    {
        Application.targetFrameRate = _fps;
        //GameStateManager.SetStageLevel(Level.Easy);
        if(_isResetSaveData)
        {
            SaveManager.DeleteAll();
        }
    }

    protected override void Destroy()
    {
       SaveManager.Save();
    }
}
