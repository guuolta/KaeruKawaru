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
    private float _maxTime;
    public float MaxTime => _maxTime;
    public float lefttime;

    private void Start()
    {
        _maxTime = StageManager.Instance.TimeLimit;
        Observable.EveryUpdate()
            .Where(_ => tActive)
            .Do(_ => timer += Time.deltaTime)
            .Subscribe().AddTo(this);
    }

    private void Update()
    {
        lefttime = MaxTime - CurrentTime;
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
