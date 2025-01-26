using UnityEngine;

/// <summary>
/// 文字だけのボタン
/// </summary>
public class TextButton : AnimationPartBase
{
    [Header("SEの種類")]
    [SerializeField]
    private SEType _seType = SEType.Posi;

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventPlaySE();
    }

    /// <summary>
    /// SEを再生するイベントを設定
    /// </summary>
    private void SetEventPlaySE()
    {
        OnClickCallback = () =>
        {
            AudioManager.Instance.PlayOneShotSE(_seType);
        };
    }
}
