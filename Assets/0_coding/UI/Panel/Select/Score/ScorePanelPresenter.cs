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
    private void SetScore()
    {
        View.SetEasyScore(ScoreManager.Instance.EasyHighScoreList);
        View.SetHardScore(ScoreManager.Instance.HardHighScoreList);
    }
    
}
