using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class Timer : MonoBehaviour
{
    private float timer;
    public float CurrentTime { get { return timer; } }
    public bool tActive = false;
    public float maxtime;

    private void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => tActive)
            .Do(_ => timer += Time.deltaTime)
            .Subscribe();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && tActive == false)
        {
            OnStart();
        }
        else if(Input.GetKeyDown(KeyCode.Space) && tActive == true)
        {
            OnStop();
        }
    }
    public void OnStart()
    {
        tActive = true;
    }
    public void OnStop()
    {
        tActive = false;
    }
    public void OnReset()
    {
        timer = 0f;
        OnStop();
    }
}