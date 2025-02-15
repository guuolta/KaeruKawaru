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

    private ReactiveProperty<int> _point = new ReactiveProperty<int>();
    /// <summary>
    /// 現在のスコア
    /// </summary>
    public IReadOnlyReactiveProperty<int> Point => _point;
    private ReactiveProperty<int> _highScoreIndex = new ReactiveProperty<int>(-1);
    /// <summary>
    /// ハイスコアを更新した順位
    /// </summary>
    public IReadOnlyReactiveProperty<int> HighScoreIndex => _highScoreIndex;
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

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        // セーブデータから前回までのハイスコア取得
        _easyHighScoreList = SaveManager.GetEasyHighScores().ToList();
        _hardHighScoreList = SaveManager.GetHardHighScores().ToList();
        
        SetEvent();
    }

    /// <summary>
    /// イベント発行
    /// </summary>
    private void SetEvent()
    {
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
                    // ゲーム開始時にゲーム中のスコアをリセット
                    case GameState.Start:
                        ResetCount();
                        break;
                    // リザルト時にハイスコアを更新
                    case GameState.Result:
                        UpdateHighScore();
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
        _point.Value = 0;
        _clearQuestionCount = 0;
        _comboBonus = 0;
        _stepBonus = 0;
        _highScoreIndex.Value = -1;
    }

    /// <summary>
    /// ハイスコアを更新
    /// </summary>
    private void UpdateHighScore()
    {
        // ハイスコアの最低値よりも低いなら終了
        if (_point.Value <= _highScoreList[_highScoreCount - 1])
        {
            return;
        }

        // ハイスコアのランキングを更新
        _highScoreList.Add(_point.Value);
        _highScoreList.Sort();
        _highScoreList.Reverse();
        _highScoreList.RemoveAt(_highScoreList.Count - 1);

        // 現在のレベルのハイスコアランキングに反映
        switch(GameStateManager.StageLevel.Value)
        {
            case Level.Easy:
                _easyHighScoreList.Clear();
                _easyHighScoreList.AddRange(_highScoreList);
                SaveManager.SetEasyHighScore(_easyHighScoreList.ToArray());
                break;
            case Level.Hard:
                _hardHighScoreList.Clear();
                _hardHighScoreList.AddRange(_highScoreList);
                SaveManager.SetHardHighScore(_hardHighScoreList.ToArray());
                break;
            default:
                break;
        }

        // ハイスコアの1位を更新したら、知らせる
        if(_highScoreList.IndexOf(_point.Value) == 0)
        {
            _highScoreIndex.Value = 0;
        }
    }

    /// <summary>
    /// UnityRoomにスコアを送信
    /// </summary>
    private void UpdateScoreToUnityRoom()
    {
        UnityroomApiClient.Instance.SendScore(GameStateManager.StageLevel.Value == Level.Hard ? 2 : 1,
            _point.Value, ScoreboardWriteMode.HighScoreDesc);

        //Debug.Log(GameStateManager.StageLevel.Value == Level.Hard ? 2 : 1);
    }

    /// <summary>
    /// レベルによるイベント設定
    /// </summary>
    private void SetEventLevel()
    {
        // 現在のレベルのランキングを公開
        GameStateManager.StageLevel
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                switch(value)
                {
                    case Level.Easy:
                        _highScoreList = new List<int>(_easyHighScoreList);
                        break;
                    case Level.Hard:
                        _highScoreList = new List<int>(_hardHighScoreList);
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
    /// <param name="stepDistance">追加する手数ボーナス</param>
    public void AddStepBonus(int stepDistance)
    {
        int bounus = _stepBonusPoint * stepDistance;
        _stepBonus += bounus;
        _point.Value += bounus;
    }

    /// <summary>
    /// ポイントを計算(コンボボーナスも計算する)
    /// </summary>
    /// <param name="score"> スコア </param>
    /// <returns></returns>
    private int CalculatePoint(List<int> score)
    {
        // 一度に加算するポイントが複数ある場合は、コンボボーナスを設定
        int point = score.Sum();
        int count = score.Count;
        
        if (count > 1)
        {
            _comboBonus += point;
        }

        // 獲得ポイントに一度にクリアしたお題数を乗算
        return point * count;
    }
}