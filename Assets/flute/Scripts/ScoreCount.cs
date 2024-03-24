using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using TMPro;

public class ScoreCount : MonoBehaviour
{
    TextMeshProUGUI scoretext;
    void Start()
    {
        scoretext = this.GetComponent<TextMeshProUGUI>();

        ScoreManager.Instance.Point.Subscribe(value => {
            Debug.Log(value);
            DoScoreAnimation(value);
        }).AddTo(this);
    }
    private void DoScoreAnimation(int score)
    {
        scoretext.SetText(score.ToString());
    }
}
