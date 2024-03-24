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
    public float lefttime;

    private void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => tActive)
            .Do(_ => timer += Time.deltaTime)
            .Subscribe().AddTo(this);
    }

    private void Update()
    {
        lefttime = maxtime - CurrentTime;
        if(GameStateManager.Status.Value == GameState.Play && tActive == false)
        {
            OnStart();
        }
        else if(GameStateManager.Status.Value != GameState.Play && tActive == true)
        {
            OnStop();
        }
        
        if(lefttime <= 0)
        {
            GameStateManager.SetGameState(GameState.Result);
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
