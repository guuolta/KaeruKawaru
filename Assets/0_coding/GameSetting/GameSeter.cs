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
    }

    protected override void Destroy()
    {
       
    }
}
