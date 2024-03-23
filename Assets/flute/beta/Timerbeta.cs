using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class Timerbeta : MonoBehaviour
{
    private float m_fTimer;
    public float CurrentTime { get { return m_fTimer; } }
    public bool m_bActive = false;
    public float maxtime;
    public float lefttime;

    private void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => m_bActive)
            .Do(_ => m_fTimer += Time.deltaTime)
            .Subscribe();
    }

    private void Update()
    {
        lefttime = maxtime - CurrentTime;
        if(Input.GetKeyDown(KeyCode.Space)&&m_bActive == false)
        {
            OnStart();
        }
        else if(Input.GetKeyDown(KeyCode.Space)&&m_bActive == true)
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
        m_bActive = true;
    }
    public void OnStop()
    {
        m_bActive = false;
    }
    public void OnReset()
    {
        m_fTimer = 0f;
        OnStop();
    }
}
