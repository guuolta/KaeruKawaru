using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using System.Text.RegularExpressions;
public class TPbeta : MonoBehaviour
{
    float timea,timeb = 10f;
    public float lefttime;
    string stra, strb;
    void Start()
    {
        var inputObservable = 
        Observable.EveryUpdate()
            .ThrottleFirst(TimeSpan.FromMilliseconds(1));

        Observable.TimeInterval(inputObservable)
            .Do(x => {
                stra = x.Interval + "";
                strb = Regex.Replace (stra, @"[^0-9]", "");
                timea = float.Parse("0."+strb) * 1000000;
            })
            .Where(_ => Input.GetMouseButton(0))
            .Where(_ => timeb >= 0)
            .Do(x => {
                timeb -= timea;
                lefttime = (float)Math.Ceiling(timeb);
                Debug.Log(lefttime);
            })
            .Where(_ => timeb <= 0)
            .Subscribe(_ => {
                Debug.Log("TimeOver");
            });
    }
}
