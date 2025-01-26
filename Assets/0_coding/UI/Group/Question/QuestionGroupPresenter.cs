using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// お題パネルをまとめたUI
/// </summary>
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
    
    /// <summary>
    /// お題パネルを追加
    /// </summary>
    /// <param name="question"></param>
    public void AddPanel(Question question)
    {
        // すでに作成済みの場合は終了
        if(_questionDict.ContainsKey(question) || _questionDict.Count >= _questionCount)
        {
            return;
        }

        // お題作成
        var panel = Instantiate(_questionPanelBase, transform);
        panel.CreateQuestionPanel(question.Trouts);
        _questionDict.Add(question, panel);
        
        // 表示
        View.AddPanel(panel);
    }

    /// <summary>
    /// お題パネルを削除
    /// </summary>
    /// <param name="question"></param>
    public void RemovePanel(Question question)
    {
        // お題が見つからない場合は終了
        if(!_questionDict.ContainsKey(question))
        {
            return;
        }

        View.RemovePanel(_questionDict[question]);
        _questionDict.Remove(question);
    }
}
