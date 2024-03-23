using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;

public class FlClickButton : FlBase {

    public Button Button;
    // Use this for initialization
    void Start () {
        gameObject.transform.position = new Vector2(0, 1f);
        //クリックされた最初の一回だけ値をプッシュ
        Button.onClick.AsObservable()//.First()
            .Subscribe(l => Move(1f, 0));
    }
}