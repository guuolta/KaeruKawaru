using UnityEngine;

/// <summary>
/// ゲームの設定
/// </summary>
public class GameSetter : DontDestroySingletonObject<GameSetter>
{
    [Header("セーブデータをリセットするか")]
    [SerializeField]
    private bool _isResetSaveData = false;
    [Header("fpsの量")]
    [SerializeField]
    private int _fps = 60;

    protected override void Awake()
    {
        base.Awake();
        
        // fps設定
        Application.targetFrameRate = _fps;
        AudioManager.Instance.Init();
        ScoreManager.Instance.Init();
        
        if(_isResetSaveData)
        {
            // セーブデータを削除する
            SaveManager.DeleteAll();
        }
    }

    // ゲーム終了時にセーブする
    private void OnDestroy()
    {
        base.OnDestroy();
       SaveManager.Save();
    }
}
