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

    protected override void Init()
    {
        base.Init();

        for (int i = 0; i < _questionCount; i++)
        {
            _questionList.Add(CreateQuestion(_rowCount, _columnCount));
            var questionPanel = Instantiate(_questionPanelBase, _questionPanelParent);
            questionPanel.CreateQuestionPanel(_questionList[i].TroutList);
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
        EvolutionaryType[][] trouts = new EvolutionaryType[rowCount][];

        for(int i= 0; i < rowCount; i++)
        {
            trouts[i] = new EvolutionaryType[columnCount];
        }

        int randomRow = Random.Range(0, rowCount);
        int randomColumn = Random.Range(0, columnCount);
        trouts[randomRow][randomColumn] = ((EvolutionaryType)Random.Range(1, 4));

        for(int i=0; i<rowCount; i++)
        {
            for (int j=0; j<columnCount; j++)
            {
                if(i == randomRow && j == randomColumn)
                    continue;
                trouts[i][j] = ((EvolutionaryType)Random.Range(0, 4));
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