using System.Collections;
using System.Collections.Generic;
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

    private List<Question> _questionList = new List<Question>();
    /// <summary>
    /// お題のリスト
    /// </summary>
    public List<Question> QuestionList => _questionList;

    protected override void Init()
    {
        base.Init();

        for (int i = 0; i < _questionCount; i++)
        {
            _questionList.Add(CreateQuestion(_rowCount, _columnCount));
            Debug.Log("point:"+_questionList[i].Point);
        }
    }

    public Question CreateQuestion(int rowCount, int columnCount)
    {
        EvolutionaryType[][] trouts = new EvolutionaryType[rowCount][];

        for(int i=0; i<rowCount; i++)
        {
            trouts[i] = new EvolutionaryType[columnCount];
            for (int j=0; j<columnCount; j++)
            {
                trouts[i][j] = ((EvolutionaryType)Random.Range(0, 3));
            }
        }

        return new Question(trouts);
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
    public EvolutionaryType[][] TroutList => _trouts;
    private int _point = 0;
    /// <summary>
    /// 点数
    /// </summary>
    public int Point => _point;

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
}