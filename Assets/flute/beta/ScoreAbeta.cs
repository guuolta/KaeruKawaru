using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class ScoreAbeta : MonoBehaviour
{
    private ReactiveProperty<int> a = new ReactiveProperty<int>();
    TextMeshProUGUI scoretext;
    void Start()
    {
        scoretext = this.GetComponent<TextMeshProUGUI>();

        /*ScoreManager.Instance.Point.Subscribe(value => {
            Debug.Log(value);
            DoScoreAnimation(value);
        }).AddTo(this);/**/

        a.Subscribe(value => {
            Debug.Log(value);
            DoScoreAnimation(value);
        }).AddTo(this);
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            a.Value += 1;
        }
    }

    private void DoScoreAnimation(int score)
    {
        scoretext.DOText(score.ToString(),0);
        if(score != 0)
        {
            scoretext.DOScale(0.5f,0.1f).SetEase(Ease.OutBack);
            scoretext.DOScale(0.4f,0.1f).SetDelay(0.1f).SetEase(Ease.Linear);
        }
    }
}
