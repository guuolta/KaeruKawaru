using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UniRx;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class QuestionManager : SingletonObjectBase<QuestionManager>
{
    [Header("お題の数")]
    [SerializeField]
    private int _questionCount = 3;
    [Header("お題の幅")]
    [SerializeField]
    private int _widthCount = 2;
    [Header("お題パネルの親オブジェクト")]
    [SerializeField]
    private QuestionGroupPresenter _questionPanelParent;

    private BoolReactiveProperty _isCheckedAnswer = new BoolReactiveProperty(true);
    /// <summary>
    /// 解答を確認したか
    /// </summary>
    public BoolReactiveProperty IsCheckedAnswer => _isCheckedAnswer;
    private ReactiveProperty<int> _activeQuestionCount = new ReactiveProperty<int>();
    /// <summary>
    /// 生成しているお題の数
    /// </summary>
    public IReadOnlyReactiveProperty<int> ActiveQuestionCount => _activeQuestionCount;
    
    private List<Question> _questionList = new List<Question>();

    protected override void Init()
    {
        base.Init();
        _questionPanelParent.SetInit(_questionCount);
        _activeQuestionCount.Value = _questionCount;
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventQuestCount();
        SetEventQuestion(_widthCount, Ct);
        SetInitPanel(_widthCount);
    }

    /// <summary>
    /// 最初のお題パネル設定
    /// </summary>
    /// <param name="widthCount">幅数</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public void SetInitPanel(int widthCount)
    {
        for (int i = 0; i < _questionCount; i++)
        {
            SetQuestion(widthCount, i);
        }
    }

    /// <summary>
    /// お題の数のイベント設定
    /// </summary>
    private void SetEventQuestCount()
    {
        Observable.EveryUpdate()
            .SkipWhile(_ => GameStateManager.Status.Value != GameState.Play)
            .TakeUntilDestroy(this)
            .Select(_ => _questionList.Count)
            .DistinctUntilChanged()
            .Where(value => _activeQuestionCount.Value != value && value <= _questionCount)
            .Subscribe(value =>
            {
                _activeQuestionCount.Value = value;
            });
    }

    /// <summary>
    /// お題のイベント設定
    /// </summary>
    /// <param name="widthCount"> 幅数 </param>
    private void SetEventQuestion(int widthCount, CancellationToken ct)
    {
        bool isWait = false;

        _activeQuestionCount
            .Skip(1)
            .TakeUntilDestroy(this)
            .Where(value => !isWait && value <= _questionCount)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                await UniTask.WaitUntil(() => _isCheckedAnswer.Value, cancellationToken: ct);
                SetQuestion(widthCount, value);
                CheckQuestion(StageManager.Instance.TroutFrogs);
            });
    }

    /// <summary>
    /// お題を設定
    /// </summary>
    /// <param name="widthCount"> 幅数 </param>
    /// <param name="index">お題番号</param>
    /// <returns></returns>
    private void SetQuestion(int widthCount, int index)
    {
        if(index < 0 || index >= _questionCount)
        {
            return;
        }

        var question = CreateQuestion(widthCount);
        _questionPanelParent.SetPanel(question);
        _questionList.Add(question);
    }

    /// <summary>
    /// お題を作成
    /// </summary>
    /// <param name="widthCount"> 幅数 </param>
    /// <returns></returns>
    public Question CreateQuestion(int widthCount)
    {
        var trouts = new EvolutionaryType[widthCount][];

        for(int i= 0; i < widthCount; i++)
        {
            trouts[i] = new EvolutionaryType[widthCount];
        }

        int randomRow = Random.Range(0, widthCount);
        int randomColumn = Random.Range(0, widthCount);
        trouts[randomRow][randomColumn] = (EvolutionaryType)Random.Range(1, 4);

        for(int i=0; i<widthCount; i++)
        {
            for (int j=0; j< widthCount; j++)
            {
                if(i == randomRow && j == randomColumn)
                    continue;
                trouts[i][j] = (EvolutionaryType)Random.Range(0, 4);
            }
        }

        return new Question(trouts);
    }

    /// <summary>
    /// 解答確認
    /// </summary>
    /// <param name="troutFrogs"> ステージのマス </param>
    public void CheckQuestion(ReactiveProperty<Frog>[][] troutFrogs)
    {
        _isCheckedAnswer.Value = false;

        if(troutFrogs.Length == 0 || troutFrogs[0].Length == 0)
        {
            return;
        }

        var scoreList = new List<int>();
        var questionList = new List<Question>();

        foreach (var question in _questionList)
        {
            if (question.CheckAnswer(troutFrogs))
            {
                scoreList.Add(question.Point);
                questionList.Add(question);

                if(question.CheckGetBonus())
                {
                    ScoreManager.Instance.AddStepBonus();
                }
            }
        }

        if(scoreList.Count > 0)
        {
            ScoreManager.Instance.AddPoint(scoreList);

            foreach (var question in questionList)
            {
                _questionPanelParent.RemovePanel(question);
                _questionList.Remove(question);
                question.Dispose();
            }
        }

        _isCheckedAnswer.Value = true;
    }

    protected override void Destroy()
    {
        base.Destroy();

        foreach (var question in _questionList)
        {
            question.Dispose();
        }
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
    public EvolutionaryType[][] Trouts => _trouts;
    private int _widthCount;
    /// <summary>
    /// マスの幅数
    /// </summary>
    public int WidthCount
    {
        get
        {
            if(_widthCount == 0)
            {
                _widthCount = _trouts.Length;
            }

            return _widthCount;
        }
    }

    private int _step = 0;
    private int _stepBonusCount = 0;
    private int _point = 0;
    /// <summary>
    /// ポイント
    /// </summary>
    public int Point => _point;
    private CompositeDisposable _disposable = new CompositeDisposable();

    public Question(EvolutionaryType[][] trouts)
    {
        if(trouts.Length == 0 || trouts[0].Length == 0)
        {
            Debug.LogError("マスがありません。");
            return;
        }

        if(trouts.Length != trouts[0].Length)
        {
            Debug.LogError("マスの幅と高さが違います。");
            return;
        }

        _trouts = trouts;
        SetStepPoint();
    }

    /// <summary>
    /// ボーナスをもらえる手数を設定
    /// </summary>
    private void SetStepPoint()
    {
        for (int i = 0; i < _trouts.Length; i++)
        {
            for (int j = 0; j < _trouts[i].Length; j++)
            {
                if (_trouts[i][j] != EvolutionaryType.None)
                {
                    _point+=100;
                    _stepBonusCount++;
                }
            }
        }

        _stepBonusCount = Mathf.Clamp(_stepBonusCount, 0, _stepBonusCount-1);

        SetEventStep();
    }

    /// <summary>
    /// 手数のイベント設定
    /// </summary>
    private void SetEventStep()
    {
        PlayerOperator.Instance.ClickCount
            .DistinctUntilChanged()
            .Subscribe(_ =>
            {
                _step++;
            }).AddTo(_disposable);
    }

    /// <summary>
    /// 解答確認
    /// </summary>
    /// <param name="trouts"> ステージの現在のマス </param>
    /// <returns></returns>
    public bool CheckAnswer(ReactiveProperty<Frog>[][] trouts)
    {
        int rowCount = trouts.Length;
        int columnCount = trouts[0].Length;
        if (rowCount < WidthCount || columnCount < WidthCount)
        {
            return false;
        }

        int rowLoopCount = rowCount - WidthCount;
        int columnLoopCount = columnCount - WidthCount;

        for (int i = 0; i <= rowLoopCount; i++)
        {
            for (int j = 0; j <= columnLoopCount; j++)
            {
                if (Trouts[0][0] == EvolutionaryType.None
                    || Trouts[0][0] == trouts[i][j].Value.Type.Value)
                {
                    bool isMatch = true;
                    for(int k = 0; k < WidthCount; k++)
                    {
                        if (!CheckRow(j, Trouts[k], trouts[i + k]))
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// お題とステージのマスの行を比較
    /// </summary>
    /// <param name="startIndex"> 比較するステージの最初の要素番号 </param>
    /// <param name="questionRows"> お題の行 </param>
    /// <param name="stageRows"> ステージの行 </param>
    /// <returns></returns>
    private bool CheckRow(int startIndex, EvolutionaryType[] questionRows, ReactiveProperty<Frog>[] stageRows)
    {
        for(int i=0; i<questionRows.Length; i++)
        {
            if (questionRows[i] != EvolutionaryType.None
                && questionRows[i] != stageRows[i+ startIndex].Value.Type.Value)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 手数のボーナスをもらえるか
    /// </summary>
    /// <returns></returns>
    public bool CheckGetBonus()
    {
        return _step <= _stepBonusCount;
    }


    /// <summary>
    /// イベント削除
    /// </summary>
    public void Dispose()
    {
        _disposable.Dispose();
    }

    private void CheckTrout()
    {
        for(int i = 0; i < _trouts.Length; i++)
        {
            for (int j = 0; j < _trouts[i].Length; j++)
            {
                Debug.Log(i + "," + j + ":"+_trouts[i][j]);
            }
        }
    }
}