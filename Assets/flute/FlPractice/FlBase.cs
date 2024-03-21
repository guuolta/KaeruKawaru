using UnityEngine;
using System.Collections;

public class FlBase : MonoBehaviour 
{

    // Use this for initialization
    void Start () 
    {

    }

    // (dx,dy)だけ移動する
    public void Move (float dx,float dy) 
    {
        gameObject.transform.position += new Vector3(dx, dy, 0);
    }
}
