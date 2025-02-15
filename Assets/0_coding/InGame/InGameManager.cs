using UnityEngine;
using UnityEngine.Serialization;

public class InGameManager : MonoBehaviour
{
    [SerializeField,Header("開始のUI")]
    private StartAnimation _startAnimation;
    [SerializeField,Header("リザルトUI")]
    private ResultUIPanelPresenter resultPanelPresenter;
    [SerializeField, Header("タイマーUI")]
    private TimerPanelPresenter timerPanel;
    [SerializeField, Header("スコアテキスト")]
    private ScoreAnimation _scoreAnimatifon;
    private void Awake()
    {
        // マネージャー系初期化
        StageManager.Instance.Init();
        QuestionManager.Instance.Init();
        PlayerOperator.Instance.Init();
        
        // UI初期化
        PausePanelManager.Instance.Init();
        resultPanelPresenter.Init();
        timerPanel.Init();
        _scoreAnimatifon.Init();
        
        _startAnimation.Init();
    }
}
