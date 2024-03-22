using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ScoreManager : DontDestroySingletonObject<ScoreManager>
{
    [Header("ハイスコアの記録数")]
    [SerializeField]
    private int _highScoreCount = 3;

    private ReactiveProperty<int> _point = new ReactiveProperty<int>();
    /// <summary>
    /// 現在のスコア
    /// </summary>
    public IReadOnlyReactiveProperty<int> Point => _point;
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

    private List<int> _highScoreList = new List<int>();

    protected override void Init()
    {
        base.Init();
        InitHighScoreList();
    }

    /// <summary>
    /// ハイスコアリストの初期化
    /// </summary>
    private void InitHighScoreList()
    {
        for(int i = 0; i < _highScoreCount; i++)
        {
            _easyHighScoreList.Add(0);
            _hardHighScoreList.Add(0);
        }
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
                        _point.Value = 0;
                        break;
                    case GameState.Result:
                        SetHighScore();
                        break;
                    default:
                        break;
                }
            });
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
    /// ハイスコアを設定
    /// </summary>
    /// <returns> ポイントがハイスコアか </returns>
    private bool SetHighScore()
    {
        if (_point.Value <= _highScoreList[_highScoreCount-1])
        {
            return false;
        }

        _highScoreList.Add(_point.Value);
        _highScoreList.Sort();
        _highScoreList.Reverse();
        _highScoreList.RemoveAt(_highScoreList.Count - 1);
        return true;
    }

    /// <summary>
    /// スコアを追加
    /// </summary>
    /// <param name="point"> 追加するポイント </param>
    public void AddPoint(List<int> point)
    {
        _point.Value += CalculatePoint(point);
        Debug.Log("point:"+_point);
    }

    /// <summary>
    /// ポイントを計算
    /// </summary>
    /// <param name="score"> スコア </param>
    /// <returns></returns>
    private int CalculatePoint(List<int> score)
    {
        int point = 0;
        foreach (var s in score)
        {
            point += s;
        }

        return point;
    }
}
