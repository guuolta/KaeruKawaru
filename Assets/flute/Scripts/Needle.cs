using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class Needle : MonoBehaviour
{
    public Timer gameTimer;
    private float a;
    RectTransform recttransform;

    void Start()
    {
        recttransform = GetComponent<RectTransform>();

        Observable.EveryUpdate()
            .Do(_ => a = gameTimer.CurrentTime / gameTimer.Maxtime)
            .Where(_ => a <= 1)
            .Subscribe(_ => 
                recttransform.rotation = Quaternion.Euler(0, 0, -360*a));
    }
}
