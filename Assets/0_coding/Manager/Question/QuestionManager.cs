using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class QuestionManager : SingletonObjectBase<QuestionManager>
{
    [Header("お題の数")]
    [SerializeField]
    private int _questionCount = 3;
    [Header("お題の行数")]
    [SerializeField]
    private int _rowCount = 2;
    [Header("お題の列数")]
    [SerializeField]
    private int _columnCount = 2;
    [Header("お題パネルの親オブジェクト")]
    [SerializeField]
    private Transform _questionPanelParent;
    [Header("お題パネル")]
    [SerializeField]
    private QuestionPanelPresenter _questionPanelBase;

    private List<Question> _questionList = new List<Question>();
    /// <summary>
    /// お題のリスト
    /// </summary>
    public List<Question> QuestionList => _questionList;
    private BoolReactiveProperty _isCheckedAnswer = new BoolReactiveProperty(false);
    /// <summary>
    /// 正解かどうか
    /// </summary>
    public BoolReactiveProperty IsCheckedAnswer => _isCheckedAnswer;

    protected override void Init()
    {
        base.Init();

        for (int i = 0; i < _questionCount; i++)
        {
            _questionList.Add(CreateQuestion(_rowCount, _columnCount));
            var questionPanel = Instantiate(_questionPanelBase, _questionPanelParent);
            questionPanel.CreateQuestionPanel(_questionList[i].Trouts);
            Debug.Log("point:"+_questionList[i].Point);
        }
    }

    /// <summary>
    /// お題を作成
    /// </summary>
    /// <param name="rowCount"> 行数 </param>
    /// <param name="columnCount"> 列数 </param>
    /// <returns></returns>
    public Question CreateQuestion(int rowCount, int columnCount)
    {
        var trouts = new EvolutionaryType[rowCount][];

        for(int i= 0; i < rowCount; i++)
        {
            trouts[i] = new EvolutionaryType[columnCount];
        }

        int randomRow = Random.Range(0, rowCount);
        int randomColumn = Random.Range(0, columnCount);
        trouts[randomRow][randomColumn] = (EvolutionaryType)Random.Range(1, 4);

        for(int i=0; i<rowCount; i++)
        {
            for (int j=0; j<columnCount; j++)
            {
                if(i == randomRow && j == randomColumn)
                    continue;
                trouts[i][j] = (EvolutionaryType)Random.Range(0, 4);
            }
        }

        return new Question(trouts);
    }

    public void CheckQuestion(EvolutionaryType[][] trouts)
    {
        var _score = new List<int>();

        
    }

    
}

/// <summary>
/// お題
/// </summary>
public class Question
{
    private EvolutionaryType[][] _trouts;
    /// <summary>
    /// マスのリスト
    /// </summary>
    public EvolutionaryType[][] Trouts => _trouts;
    private int _point = 0;
    /// <summary>
    /// 点数
    /// </summary>
    public int Point => _point;
    private int _rowCount;
    /// <summary>
    /// マスの行数
    /// </summary>
    public int RowCount
    {
        get
        {
            if(_rowCount == 0)
            {
                _rowCount = _trouts.Length;
            }

            return _rowCount;
        }
    }
    private int _columnCount;
    /// <summary>
    /// マスの列数
    /// </summary>
    public int ColumnCount
    {
        get
        {
            if(_columnCount == 0)
            {
                _columnCount = _trouts[0].Length;
            }

            return _columnCount;
        }
    }

    public Question(EvolutionaryType[][] trouts)
    {
        _trouts = trouts;
        
        for(int i=0; i<trouts.Length; i++)
        {
            for(int j=0; j < trouts[i].Length; j++)
            {
                switch (trouts[i][j])
                {
                    case EvolutionaryType.Egg:
                        _point += 1;
                        break;
                    case EvolutionaryType.Tadpole:
                        _point += 2;
                        break;
                    case EvolutionaryType.Frog:
                        _point += 3;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 解答確認
    /// </summary>
    /// <param name="trouts"> ステージの現在のマス </param>
    /// <returns></returns>
    public bool CheckAnswer(EvolutionaryType[][] trouts)
    {
        int rowCount = trouts.Length;
        int columnCount = trouts[0].Length;
        if (rowCount < RowCount || columnCount < ColumnCount)
        {
            return false;
        }

        for (int i = 0; i <= rowCount - RowCount; i++)
        {
            if (!CheckRow(0, trouts[i]))
            {
                continue;
            }

            bool isMatch = true;
            for (int j = 1; j < rowCount; j++)
            {
                if (!CheckRow(j, trouts[i + j]))
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 行があっているか
    /// </summary>
    /// <param name="index"> 対象の行番号 </param>
    /// <param name="rows"> 確かめる行の要素 </param>
    /// <returns></returns>
    private bool CheckRow(int index, EvolutionaryType[] rows)
    {
        int _zeroCount = 0;
        for (int i = 0; i < rows.Length - ColumnCount; i++)
        {
            if (Trouts[index][i] == 0)
            {
                _zeroCount++;
                continue;
            }

            if (Trouts[index][i] != rows[0])
                continue;

            bool isMatch = true;
            for (int j = 1; j < ColumnCount; j++)
            {
                if (Trouts[index][i + j] != rows[j])
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch)
            {
                return true;
            }
        }

        if (_zeroCount == rows.Length - ColumnCount)
        {
            return true;
        }

        return false;
    }
}