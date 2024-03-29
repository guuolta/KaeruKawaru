using UniRx;
using UnityEngine;

public class Board : GameObjectBase
{
    [Header("マスオブジェクト")]
    [SerializeField]
    private GameObject _cell;
    [Header("カエルオブジェクト")]
    [SerializeField]
    private Frog _frog;
    [Header("マスとカエルオブジェクトの余白")]
    [SerializeField]
    private float _margin = 0.1f;

    private Frog[][] _troutFrogs;
    /// <summary>
    /// マスのカエル
    /// </summary>
    public Frog[][] TroutFrogs => _troutFrogs;

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

        float frogSizeX = (1 - _margin) / _frog.Transform.localScale.x;
        float frogSizeZ = (1 - _margin) / _frog.Transform.localScale.z;
        float minSize = Mathf.Min(frogSizeX, frogSizeZ);

        _troutFrogs = new Frog[rowCount][];
        for (int i = 0; i < rowCount; i++)
        {
            _troutFrogs[i] = new Frog[columnCount];
            for (int j = 0; j < columnCount; j++)
            {
                _troutFrogs[i][j] = new Frog();
                var cell = Instantiate(_cell, Transform);
                cell.transform.localPosition = new Vector3(iniPosX + j * sizeX, 0, iniPosZ - i * sizeZ);
                cell.transform.localScale = new Vector3(sizeX, 1, sizeZ);

                var frog = Instantiate(_frog, cell.transform);
                frog.Transform.localScale = new Vector3(frog.Transform.localScale.x * minSize, 10f, frog.Transform.localScale.z * minSize);
                frog.Transform.localPosition = new Vector3(0, frog.Transform.localScale.y/2, 0);

                _troutFrogs[i][j] = frog;
            }
        }
    }
}
