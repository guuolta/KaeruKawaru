using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using unityroom.Api;

public class ScoreManager : DontDestroySingletonObject<ScoreManager>
{
    [Header("ハイスコアの記録数")]
    [SerializeField]
    private int _highScoreCount = 3;
    [Header("手数ボーナス")]
    [SerializeField]
    private int _stepBonusPoint = 50;

    private BoolReactiveProperty _isUpdateHighScore = new BoolReactiveProperty(false);
    /// <summary>
    /// ハイスコア更新フラグ
    /// </summary>
    public BoolReactiveProperty IsUpdateHighScore => _isUpdateHighScore;

    private ReactiveProperty<int> _point = new ReactiveProperty<int>();
    /// <summary>
    /// 現在のスコア
    /// </summary>
    public IReadOnlyReactiveProperty<int> Point => _point;
    private List<int> _highScoreList = new List<int>();
    /// <summary>
    /// ハイスコアリスト
    /// </summary>
    public List<int> HighScoreList => _highScoreList;
    private List<int> _easyHighScoreList = new List<int>();
    /// <summary>
    /// イージーモードのハイスコアリスト
    /// </summary>
    public List<int> EasyHighScoreList => _easyHighScoreList;
    private List<int> _hardHighScoreList = new List<int>();
    /// <summary>
    /// ハードモードのハイスコアリスト
    /// </summary>
    public List<int> HardHighScoreList => _hardHighScoreList;

    private int _clearQuestionCount = 0;
    /// <summary>
    /// クリアしたお題の数
    /// </summary>
    public int ClearQuestionCount => _clearQuestionCount;
    private int _comboBonus = 0;
    /// <summary>
    /// コンボボーナス
    /// </summary>
    public int ComboBonus => _comboBonus;
    private int _stepBonus = 0;
    /// <summary>
    /// 手数ボーナス
    /// </summary>
    public int StepBonus => _stepBonus;


    protected override void Init()
    {
        base.Init();
        _easyHighScoreList = SaveManager.GetEasyHighScores().ToList();
        _hardHighScoreList = SaveManager.GetHardHighScores().ToList();
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventState();
        SetEventLevel();
    }

    /// <summary>
    /// ステートによるイベント設定
    /// </summary>
    private void SetEventState()
    {
        GameStateManager.Status
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                switch(value)
                {
                    case GameState.Start:
                        ResetCount();
                        break;
                    case GameState.Result:
                        SetHighScore();
                        UpdateScoreToUnityRoom();
                        break;
                    default:
                        break;
                }
            });
    }

    /// <summary>
    /// 値の初期化
    /// </summary>
    private void ResetCount()
    {
        _isUpdateHighScore.Value = false;
        _point.Value = 0;
        _clearQuestionCount = 0;
        _comboBonus = 0;
        _stepBonus = 0;
    }

    /// <summary>
    /// ハイスコアを設定
    /// </summary>
    /// <returns> ポイントがハイスコアか </returns>
    private void SetHighScore()
    {
        if (_point.Value <= _highScoreList[_highScoreCount - 1])
        {
            return;
        }

        _highScoreList.Add(_point.Value);
        _highScoreList.Sort();
        _highScoreList.Reverse();
        _highScoreList.RemoveAt(_highScoreList.Count - 1);

        switch(GameStateManager.StageLevel.Value)
        {
            case Level.Easy:
                _easyHighScoreList = _highScoreList;
                break;
            case Level.Hard:
                _hardHighScoreList = _highScoreList;
                break;
            default:
                break;
        }

        _isUpdateHighScore.Value = true;
    }

    /// <summary>
    /// UnityRoomにスコアを送信
    /// </summary>
    private void UpdateScoreToUnityRoom()
    {
        UnityroomApiClient.Instance.SendScore(1, _point.Value, ScoreboardWriteMode.HighScoreDesc);
    }

    /// <summary>
    /// レベルによるイベント設定
    /// </summary>
    private void SetEventLevel()
    {
        GameStateManager.StageLevel
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                switch(value)
                {
                    case Level.Easy:
                        _highScoreList = _easyHighScoreList;
                        break;
                    case Level.Hard:
                        _highScoreList = _hardHighScoreList;
                        break;
                    default:
                        break;
                }
            });
    }

    /// <summary>
    /// スコアを追加
    /// </summary>
    /// <param name="point"> 追加するポイント </param>
    public void AddPoint(List<int> point)
    {
        _point.Value += CalculatePoint(point);
        _clearQuestionCount += point.Count;
    }

    /// <summary>
    /// 手数ボーナス追加
    /// </summary>
    public void AddStepBonus(int _stepDistance)
    {
        int bounus = _stepBonusPoint * _stepDistance;
        _stepBonus += bounus;
        _point.Value += bounus;
    }

    /// <summary>
    /// ポイントを計算
    /// </summary>
    /// <param name="score"> スコア </param>
    /// <returns></returns>
    private int CalculatePoint(List<int> score)
    {
        int point = score.Sum();
        int count = score.Count;
        if (count > 1)
        {
            _comboBonus += point;
        }

        return point * count;
    }
}