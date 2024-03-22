using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class QuestionGroupPresenter : PresenterBase<QuestionGroupView>
{
    [Header("お題パネル")]
    [SerializeField]
    private QuestionPanelPresenter _questionPanelBase;

    private int _questionCount;
    private Dictionary<Question, QuestionPanelPresenter> _questionDict = new Dictionary<Question, QuestionPanelPresenter>();

    protected override void Init()
    {
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    public void SetInit(int questCount)
    {
        View.SetInit(questCount);
        _questionCount = questCount;
    }
    
    public void SetPanel(Question question)
    {
        if(_questionDict.ContainsKey(question) || _questionDict.Count >= _questionCount)
        {
            return;
        }

        var panel = Instantiate(_questionPanelBase, transform);
        panel.CreateQuestionPanel(question.Trouts);
        View.SetPanel(panel);
        _questionDict.Add(question, panel);
    }

    public void RemovePanel(Question question)
    {
        if(!_questionDict.ContainsKey(question))
        {
            return;
        }

        View.RemovePanel(_questionDict[question]);
        _questionDict.Remove(question);
    }
}
