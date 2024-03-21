using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class FlSubscribeSample : MonoBehaviour
{

    // Use this for initialization
    void Start () {

        //Returnで(0,1)という値をSubscribe内に流し込んでる
        Observable.Return(new Vector2(0, 1))
            .Subscribe(v => gameObject.transform.position = v);

        //UpdateAsObservableでUpdateのたびに値流しこんでる
        this.UpdateAsObservable().Subscribe(_ => Move(0.01f, 0));
//イベントだと this.update += _=>Move(0.01f, 0)という感じ(updateなんてイベントないけど)
    }


    /// <summary>
    /// (dx,dy)だけ移動させる
    /// </summary>
    /// <param name="dx"></param>
    /// <param name="dy"></param>
    public void Move(float dx, float dy)
    {
        gameObject.transform.position += new Vector3(dx, dy, 0);
    }
}
