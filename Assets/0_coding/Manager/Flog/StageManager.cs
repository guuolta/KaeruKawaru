using System.Threading;
using UniRx;
using UnityEngine;

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

    private ReactiveProperty<Frog>[][] _troutFrogs => _board.TroutFrogs;
    /// <summary>
    /// マスのカエル
    /// </summary>
    public ReactiveProperty<Frog>[][] TroutFrogs => _troutFrogs;

    protected override void Init()
    {
        base.Init();
        _board.CreateBoard(_row, _column);
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