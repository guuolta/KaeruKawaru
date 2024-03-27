using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePanelView : SelectPanelViewBase
{
    [Header("イージースコアリスト(昇順)")]
    [SerializeField]
    private List<TMP_Text> _easyScoreTextList = new List<TMP_Text>();

    [Header("ハードスコアリスト(昇順)")]
    [SerializeField]
    private List<TMP_Text> _hardScoreTextList = new List<TMP_Text>();

    public void SetEasyScore(List<int> scoreList)
    {
        for(int i=0;i<_easyScoreTextList.Count;i++)
        {
            _easyScoreTextList[i].text = scoreList[i].ToString();
        }
    }
    public void SetHardScore(List<int> scoreList)
    {
        for(int i=0;i<_hardScoreTextList.Count;i++)
        {
            _hardScoreTextList[i].text = scoreList[i].ToString();
        }
    }
}
