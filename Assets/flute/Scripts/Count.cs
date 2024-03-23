using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class Count : MonoBehaviour
{
    float starttime;
    string strFormat = "{0:0}";
    public Timer gameTimer;
    private TextMeshProUGUI txt;

    private void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
        starttime = gameTimer.maxtime;

        Observable.EveryUpdate()
            .Do(_ => {
                float txtTime = Mathf.Clamp(gameTimer.lefttime, 0f, starttime);
                txt.text = string.Format(strFormat, txtTime);
            })
            .Subscribe();
    }
}
