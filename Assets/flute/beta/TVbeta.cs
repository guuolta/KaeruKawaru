using System.Collections;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TVbeta : MonoBehaviour//ViewBase
{
    private Text timerText;
    TPbeta tpbeta;
    public void Start()//SetEventClick()
    {
        timerText = GetComponentInChildren<Text>();
        Observable.EveryUpdate()
            .Subscribe(_ => {
                timerText.text = tpbeta.lefttime.ToString();
            });
    }
    /*public override UniTask ShowAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }

    public override UniTask HideAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }/**/
}
