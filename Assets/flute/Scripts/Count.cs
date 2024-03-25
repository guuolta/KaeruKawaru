using TMPro;
using UniRx;
using UnityEngine;

public class Count : MonoBehaviour
{
    private float starttime => gameTimer.MaxTime;
    string strFormat = "{0:0}";
    public Timer gameTimer;
    private TextMeshProUGUI txt;

    private void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();

        Observable.EveryUpdate()
            .Do(_ => {
                float txtTime = Mathf.Clamp(gameTimer.lefttime, 0f, starttime);
                txt.text = string.Format(strFormat, txtTime);
            })
            .Subscribe().AddTo(this);
    }
}
