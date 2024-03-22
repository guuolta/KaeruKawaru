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
    [Header("制限時間")]
    [SerializeField]
    private float _timeLimit = 15f;

    protected override void Init()
    {
        Application.targetFrameRate = _fps;
        GameStateManager.SetStageLevel(Level.Easy);
        GameStateManager.SetGameState(GameState.Start);
    }

    protected async override void SetEvent()
    {
        await UniTask.WaitForSeconds(2f, cancellationToken: Ct);
        GameStateManager.SetGameState(GameState.Play);

        await UniTask.WaitForSeconds(_timeLimit, cancellationToken: Ct);
        GameStateManager.SetGameState(GameState.Result);
    }

    protected override void Destroy()
    {
       AudioManager.Instance.SaveVolume();
       SaveManager.Save();
    }
}
