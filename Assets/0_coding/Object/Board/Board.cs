using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : GameObjectBase
{
    [Header("マスオブジェクト")]
    [SerializeField]
    private GameObject _cell;

    /// <summary>
    /// 盤面を作成
    /// </summary>
    /// <param name="rowCount"> 行数 </param>
    /// <param name="columnCount"> 列数 </param>
    public void CreateBoard(int rowCount, int columnCount)
    {
        if(rowCount != columnCount)
        {
            Transform.localScale = rowCount < columnCount
                ? new Vector3(Transform.localScale.x, Transform.localScale.y, Transform.localScale.z * rowCount / columnCount)
                : new Vector3(Transform.localScale.x * columnCount / rowCount, Transform.localScale.y, Transform.localScale.z);
        }

        float sizeX = 1f / columnCount;
        float sizeZ = 1f / rowCount;
        float iniPosX = -0.5f + sizeX / 2;
        float iniPosZ = 0.5f - sizeZ / 2;
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                var cell = Instantiate(_cell, Transform);
                cell.transform.localPosition = new Vector3(iniPosX + j * sizeX, 0, iniPosZ - i * sizeZ);
                cell.transform.localScale = new Vector3(sizeX, 1, sizeZ);
            }
        }
    }
}
