using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using TMPro;
using DG.Tweening;

public class StartAnimation : MonoBehaviour
{
    TextMeshProUGUI readytext, starttext;
    void Start()
    {
        readytext = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        starttext = this.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        Observable.EveryUpdate()
            .Where(_ => GameStateManager.Status.Value == GameState.Start)
            .First()
            .Do(_ => DoStartAnimation())
            .Delay(TimeSpan.FromSeconds(1.5f))
            .Subscribe(_ => 
                GameStateManager.SetGameState(GameState.Play)
            ).AddTo(this);
    }

    public void DoStartAnimation()
    {
        readytext.DOText("よーい",0.2f).SetEase(Ease.Linear);
        readytext.DOFade(0,0.2f).SetDelay(0.7f);
        starttext.DOText("どん!!",0f).SetDelay(0.9f);
        starttext.DOScale(1.2f,0.2f).SetDelay(0.9f).SetEase(Ease.OutExpo);
        starttext.DOFade(0,0.2f).SetDelay(1.3f);
    }
}
