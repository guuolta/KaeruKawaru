using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class StageManager : SingletonObjectBase<StageManager>
{
    [Header("マス")]
    [SerializeField]
    private List<StageTrout> _stageTrouts = new List<StageTrout>();
    [Header("盤面")]
    [SerializeField]
    private Board _board;

    private ReactiveProperty<Frog>[][] _troutFrogs => _board.TroutFrogs;
    /// <summary>
    /// マスのカエル
    /// </summary>
    public ReactiveProperty<Frog>[][] TroutFrogs => _troutFrogs;

    protected override void Init()
    {
        base.Init();

        int row = 0;
        int column = 0;
        foreach(var stageTrout in _stageTrouts)
        {
            if (stageTrout.Level == GameStateManager.StageLevel.Value)
            {
                row = stageTrout.RowCount;
                column = stageTrout.ColumnCount;
            }
        }

        _board.CreateBoard(row, column);

        GameStateManager.SetGameState(GameState.Play);
        AudioManager.Instance.PlayBGM(BGMType.Main);
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventTrouts(Ct);
    }

    /// <summary>
    /// マスの進化状態のイベント設定
    /// </summary>
    private void SetEventTrouts(CancellationToken ct)
    {
        foreach(var trouts in _troutFrogs)
        {
            foreach(var trout in trouts)
            {
                trout.Value.Type
                    .TakeUntilDestroy(this)
                    .Skip(1)
                    .DistinctUntilChanged()
                    .Subscribe(value =>
                    {
                        QuestionManager.Instance.CheckQuestion(_troutFrogs);
                    });
            }        
        }
    }

    private void Check()
    {
        for (int i = 0; i < _troutFrogs.Length; i++)
        {
            for (int j = 0; j < _troutFrogs[i].Length; j++)
            {
                Debug.Log(i + "," + j + ":" + _troutFrogs[i][j].Value);
            }
        }
    }
}

[System.Serializable]
public class StageTrout
{
    [Header("レベル")]
    [SerializeField]
    private Level _level;
    /// <summary>
    /// レベル
    /// </summary>
    public Level Level => _level;
    [Header("マスの行")]
    [SerializeField]
    private int _rowCount;
    /// <summary>
    /// マスの行
    /// </summary>
    public int RowCount => _rowCount;
    [Header("マスの列")]
    [SerializeField]
    private int _columnCount;
    /// <summary>
    /// マスの列
    /// </summary>
    public int ColumnCount => _columnCount;
}