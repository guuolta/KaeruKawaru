using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class Needlebeta : MonoBehaviour
{
    public Timerbeta m_gameTimer;
    //public float m_fTargetTime;
    private float a;
    //ShowImagebeta showimagebeta;
    RectTransform recttransform;

    void Start()
    {
        recttransform = GetComponent<RectTransform>();

        Observable.EveryUpdate()
            .Do(_ => a = m_gameTimer.CurrentTime/m_gameTimer.maxtime)
            .Where(_ => a <= 1)
            .Subscribe(_ => 
                recttransform.rotation = Quaternion.Euler(0, 0, -360*a));
    }
    /*void Update()
    {
        //a = m_gameTimer.CurrentTime/m_fTargetTime;
        a = m_gameTimer.CurrentTime/m_gameTimer.maxtime;
        if(a <= 1)
        {
            recttransform.rotation = Quaternion.Euler(0, 0, -360*a);
        }
        else
        {
            recttransform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }/**/
}
