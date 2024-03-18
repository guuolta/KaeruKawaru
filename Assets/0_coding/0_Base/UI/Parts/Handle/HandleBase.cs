using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleBase : AnimationPartBase
{
    bool _isPlaySe = true;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (_isPlaySe)
        {
            AudioManager.Instance.PlayOneShotSE(SEType.Posi);
        }
    }

    public void SetOffSe()
    {
        _isPlaySe = false;
    }
}
