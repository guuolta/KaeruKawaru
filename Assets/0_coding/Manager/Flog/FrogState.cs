using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogState : MonoBehaviour//AllFrogから数字を受け取って状態変更
{
    int FrogStateNum = 0;
    GameObject Frog,tamago,otama,kaeru;
    AllFrog allfrog;
    void Start()
    {
        Frog = transform.parent.gameObject;
        allfrog = Frog.GetComponent<AllFrog>();

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

        if(FrogStateNum == 1)
        {
            tamago.SetActive(true);
            otama.SetActive(false);
            kaeru.SetActive(false);
        }
        if(FrogStateNum == 2)
        {
            tamago.SetActive(false);
            otama.SetActive(true);
            kaeru.SetActive(false);
        }
        if(FrogStateNum == 3)
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
            case "Frog1" : FrogStateNum = allfrog.arrayP[0,0]; break;
            case "Frog2" : FrogStateNum = allfrog.arrayP[0,1]; break;
            case "Frog3" : FrogStateNum = allfrog.arrayP[0,2]; break;
            case "Frog4" : FrogStateNum = allfrog.arrayP[1,0]; break;
            case "Frog5" : FrogStateNum = allfrog.arrayP[1,1]; break;
            case "Frog6" : FrogStateNum = allfrog.arrayP[1,2]; break;
            case "Frog7" : FrogStateNum = allfrog.arrayP[2,0]; break;
            case "Frog8" : FrogStateNum = allfrog.arrayP[2,1]; break;
            case "Frog9" : FrogStateNum = allfrog.arrayP[2,2]; break;
            default : break;
        }
    }
}
