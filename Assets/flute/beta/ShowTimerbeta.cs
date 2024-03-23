using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ShowTimerbeta : MonoBehaviour
{
    //public float m_fStartTime = 30;
    float starttime;
    string m_strFormat = "{0:0}";
    public Timerbeta m_gameTimer;

    private Text m_txt;

    private void Start()
    {
        m_txt = GetComponent<Text>();
        starttime = m_gameTimer.maxtime;

        Observable.EveryUpdate()
            .Do(_ => {
                float fShowTime = Mathf.Clamp(starttime - m_gameTimer.CurrentTime, 0f, starttime);
                m_txt.text = string.Format(m_strFormat, fShowTime);
            })
            .Subscribe();
    }
}
