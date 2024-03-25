using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Gauge : MonoBehaviour
{
    public Timer gameTimer;
    public Image imgTarget;

    void Start()
    {
        Observable.EveryUpdate()
            .Subscribe(_ => 
                imgTarget.fillAmount = gameTimer.CurrentTime / gameTimer.MaxTime);
    }
}
