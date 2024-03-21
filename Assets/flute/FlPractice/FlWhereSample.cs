using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FlWhereSample : FlBase {

    // Use this for initialization
    void Start () {
        //Returnで(0,1)という値をSubscribe内に流し込んでる
        Observable.Return(new Vector2(0, 1f)).Subscribe(v => gameObject.transform.position = v);

        //Whereで左クリックの間しか値がプッシュされないようにしている
        this.UpdateAsObservable()
            .Select(_ => 0.5f)
            .Where(_ => Input.GetMouseButton(0))
            .TakeWhile(l => gameObject.transform.position.x <= 2)
            .Subscribe(l => Move(0.01f, 0));
    }

}