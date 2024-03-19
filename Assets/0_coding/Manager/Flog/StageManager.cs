using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class StageManager : SingletonObjectBase<StageManager>
{
    [Header("マスの行数")]
    [SerializeField]
    private int _row = 3;
    [Header("マスの列数")]
    [SerializeField]
    private int _column = 3;
    [Header("盤面")]
    [SerializeField]
    private Board _board;

    protected override void Init()
    {
        base.Init();
        _board.CreateBoard(_row, _column);
    }
}