using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;

public class FlSubscribeSample : FlBase {

    public Button Button;
    int time = 10;

    // Use this for initialization
    void Start () {
        //Returnで(0,1)という値をSubscribe内に流し込んでる
        Observable.Return(new Vector2(0, 1f)).Subscribe(v => gameObject.transform.position = v);

        /*Observable.Timer(TimeSpan.FromSeconds(3))
            .Subscribe(_ => Destroy(gameObject));/**/

        //Whereで左クリックの間しか値がプッシュされないようにしている
        this.UpdateAsObservable()
            //.Select(_ => 0.5f)
            //.SkipWhile(_ => !Input.GetKeyDown(KeyCode.Space))
            .Where(_ => Input.GetMouseButton(0))
            //.TakeWhile(l => gameObject.transform.position.x <= 2)
            .TakeWhile(_ => time >= 0)
            .TakeUntilDestroy(this)
            .ThrottleFirst(TimeSpan.FromMilliseconds(1000f))
            .Subscribe(_ => Count(time--));
            //.Subscribe(l => Move(0.1f, 0));

        Button.onClick.AsObservable()//.First()
            .TakeUntilDestroy(this)
            .Subscribe(_ => Count(time = 10));
            //.Subscribe(l => Move(-1f, 0));

        void Count(int time)
        {
            Debug.Log(time);
        }
    }

}
