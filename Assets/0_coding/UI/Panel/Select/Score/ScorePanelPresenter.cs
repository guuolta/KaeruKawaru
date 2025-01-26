using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ScorePanelPresenter : SelectPanelPresenterBase<ScorePanelView>
{
    
    public override async UniTask ShowAsync(CancellationToken ct)
    {
        SetScore();
        await base.ShowAsync(ct);
    }
    
    /// <summary>
    /// スコアの設定
    /// </summary>
    private void SetScore()
    {
        View.SetEasyScore(ScoreManager.Instance.EasyHighScoreList);
        View.SetHardScore(ScoreManager.Instance.HardHighScoreList);
    }
    
}
