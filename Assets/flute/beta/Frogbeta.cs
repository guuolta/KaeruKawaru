using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frogbeta : MonoBehaviour
{
    int FrogState = 0;
    GameObject Frog,tamago,otama,kaeru;
    ChangeStatebeta changestatebeta;
    void Start()
    {
        Frog = transform.parent.gameObject;
        changestatebeta = Frog.GetComponent<ChangeStatebeta>();

        tamago = transform.GetChild(0).gameObject;
        otama = transform.GetChild(1).gameObject;
        kaeru = transform.GetChild(2).gameObject;
        tamago.SetActive(false);
        otama.SetActive(false);
        kaeru.SetActive(false);
    }

    void Update()
    {
        GetNumber();

        if(FrogState == 1)
        {
            tamago.SetActive(true);
            otama.SetActive(false);
            kaeru.SetActive(false);
        }
        if(FrogState == 2)
        {
            tamago.SetActive(false);
            otama.SetActive(true);
            kaeru.SetActive(false);
        }
        if(FrogState == 3)
        {
            tamago.SetActive(false);
            otama.SetActive(false);
            kaeru.SetActive(true);
        }
    }

    void GetNumber()
    {
        switch(name)
        {
            case "Frog1" : FrogState = changestatebeta.arrayP[0,0]; break;
            case "Frog2" : FrogState = changestatebeta.arrayP[0,1]; break;
            case "Frog3" : FrogState = changestatebeta.arrayP[0,2]; break;
            case "Frog4" : FrogState = changestatebeta.arrayP[1,0]; break;
            case "Frog5" : FrogState = changestatebeta.arrayP[1,1]; break;
            case "Frog6" : FrogState = changestatebeta.arrayP[1,2]; break;
            case "Frog7" : FrogState = changestatebeta.arrayP[2,0]; break;
            case "Frog8" : FrogState = changestatebeta.arrayP[2,1]; break;
            case "Frog9" : FrogState = changestatebeta.arrayP[2,2]; break;
            default : break;
        }
    }
}
