using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllFrog : MonoBehaviour//全カエルの管理
{
    GameObject Frog1,Frog2,Frog3,Frog4,Frog5,Frog6,Frog7,Frog8,Frog9;
    public int[,] arrayP = {{1,1,1},{1,1,1},{1,1,1}};
    void Start()
    {
        Frog1 = transform.GetChild(0).gameObject;
        Frog2 = transform.GetChild(1).gameObject;
        Frog3 = transform.GetChild(2).gameObject;
        Frog4 = transform.GetChild(3).gameObject;
        Frog5 = transform.GetChild(4).gameObject;
        Frog6 = transform.GetChild(5).gameObject;
        Frog7 = transform.GetChild(6).gameObject;
        Frog8 = transform.GetChild(7).gameObject;
        Frog9 = transform.GetChild(8).gameObject;
    }

    void Update()
    {
        for(int i=0;i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                if(arrayP[i,j] >= 4)
                {
                    arrayP[i,j] = 1;
                }
            }
        }
    }

    public void Evolve(RaycastHit hit)
    {
        switch(hit.collider.gameObject.name)
        {
            case "Frog1" : arrayP[0,0] += 1; break;
            case "Frog2" : arrayP[0,1] += 1; break;
            case "Frog3" : arrayP[0,2] += 1; break;
            case "Frog4" : arrayP[1,0] += 1; break;
            case "Frog5" : arrayP[1,1] += 1; break;
            case "Frog6" : arrayP[1,2] += 1; break;
            case "Frog7" : arrayP[2,0] += 1; break;
            case "Frog8" : arrayP[2,1] += 1; break;
            case "Frog9" : arrayP[2,2] += 1; break;
            default : break;
        }
    }
}
