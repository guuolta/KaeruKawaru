using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
