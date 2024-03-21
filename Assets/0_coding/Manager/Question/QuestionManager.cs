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
    private Transform _questionPanelParent;
    [Header("お題パネル")]
    [SerializeField]
    private QuestionPanelPresenter _questionPanelBase;
    [Header("お題パネルの余白")]
    [SerializeField]
    private float _margin;
    [Header("お題パネルの間隔")]
    [SerializeField]
    private float _padding;

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
    
    private float _panelIniPosY;
    private List<float> _panelPosList = new List<float>();
    private Dictionary<Question, QuestionPanelPresenter> _questionDict = new Dictionary<Question, QuestionPanelPresenter>();

    protected override void Init()
    {
        base.Init();
        _activeQuestionCount.Value = _questionCount;
        SetQuestionPanel();
        SetInitPanel(_widthCount, Ct).Forget();
    }

    /// <summary>
    /// お題パネルの設定
    /// </summary>
    private void SetQuestionPanel()
    {
        var parentRectTransform = _questionPanelParent.GetComponent<RectTransform>();
        var panelRectTransform = _questionPanelBase.GetComponent<RectTransform>();

        float sizeX = parentRectTransform.rect.width - _margin * 2;
        float sizeY = (parentRectTransform.rect.height - (_margin * 2 + _padding * (_questionCount - 1))) / _questionCount;
        float size = Mathf.Min(sizeX, sizeY);
        panelRectTransform.sizeDelta = new Vector2(size, size);

        float interval = _padding + size/2;
        float topPos = parentRectTransform.rect.height / 2 - _margin - size/2;

        for(int i = 0; i < _questionCount; i++)
        {
            _panelPosList.Add(topPos - interval * i);
        }

        _panelIniPosY = -(parentRectTransform.rect.height / 2 + size);
    }

    /// <summary>
    /// 最初のお題パネル設定
    /// </summary>
    /// <param name="widthCount">幅数</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask SetInitPanel(int widthCount, CancellationToken ct)
    {
        var tasks = new List<UniTask>();

        for (int i = 0; i < _questionCount; i++)
        {
            tasks.Add(SetQuestionAsync(widthCount, i, ct));
        }

        await UniTask.WhenAll(tasks);
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventQuestCount();
        SetEventQuestion(_widthCount, Ct);
    }

    /// <summary>
    /// お題の数のイベント設定
    /// </summary>
    private void SetEventQuestCount()
    {
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Select(_ => _questionDict.Count)
            .DistinctUntilChanged()
            .Where(value => _activeQuestionCount.Value != value && value <= _questionCount)
            .Subscribe(value =>
            {
                Debug.Log(value);
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
                await SetQuestionAsync(widthCount, value, ct);
            });
    }

    /// <summary>
    /// お題を設定
    /// </summary>
    /// <param name="widthCount"> 幅数 </param>
    /// <param name="index">お題番号</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask SetQuestionAsync(int widthCount, int index, CancellationToken ct)
    {
        if(index < 0 || index >= _questionCount)
        {
            return;
        }

        var question = CreateQuestion(widthCount);
        var questionPanel = CreateQuestionPanel(question);
        _questionDict.Add(question, questionPanel);
        await questionPanel.ShowAsync(_panelPosList[index], ct);
    }

    /// <summary>
    /// お題パネルを作成
    /// </summary>
    /// <param name="question"> お題 </param>
    /// <returns></returns>
    private QuestionPanelPresenter CreateQuestionPanel(Question question)
    {
        var questionPanel = Instantiate(_questionPanelBase, _questionPanelParent);
        questionPanel.transform.localPosition = new Vector3(0, _panelIniPosY, 0);
        questionPanel.CreateQuestionPanel(question.Trouts);
        
        return questionPanel;
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
    public async UniTask CheckQuestionAsync(ReactiveProperty<Frog>[][] troutFrogs, CancellationToken ct)
    {
        _isCheckedAnswer.Value = false;

        if(troutFrogs.Length == 0 || troutFrogs[0].Length == 0)
        {
            return;
        }

        var scoreList = new List<int>();
        var questionList = new List<Question>();

        foreach (var question in _questionDict)
        {
            if (question.Key.CheckAnswer(troutFrogs))
            {
                Debug.Log("Clear");
                scoreList.Add(question.Key.GetPoint(GetFrogList(troutFrogs)));
                questionList.Add(question.Key);
            }
        }

        if(scoreList.Count > 0)
        {
            int point = CalculatePoint(scoreList);
            ScoreManager.Instance.AddPoint(point);

            await HideClearQuestionAsync(questionList, ct);
        }

        _isCheckedAnswer.Value = true;
    }

    /// <summary>
    /// カエルのリストを取得
    /// </summary>
    /// <param name="troutFrogs"> カエルのマス </param>
    /// <returns></returns>
    private List<Frog> GetFrogList(ReactiveProperty<Frog>[][] troutFrogs)
    {
        var frogList = new List<Frog>();
        foreach(var trouts in troutFrogs)
        {
            foreach(var trout in trouts)
            {
                if(trout.Value.IsCorrect.Value)
                {
                    frogList.Add(trout.Value);
                    trout.Value.SetCorrect(false);
                }
            }
        }

        return frogList;
    }

    /// <summary>
    /// ポイントを計算
    /// </summary>
    /// <param name="score"> スコア </param>
    /// <returns></returns>
    private int CalculatePoint(List<int> score)
    {
        int point = 0;
        foreach(var s in score)
        {
            point += s;
        }

        return point;
    }

    /// <summary>
    /// クリアしたお題を非表示
    /// </summary>
    /// <param name="clearQuestionList">クリアしたお題</param>
    /// <param name="ct"></param>
    private async UniTask HideClearQuestionAsync(List<Question> clearQuestionList, CancellationToken ct)
    {
        var tasks = new List<UniTask>();

        foreach (var question in clearQuestionList)
        {
            var questionPanel = _questionDict[question];
            tasks.Add(questionPanel.HideAsync(ct));
            _questionDict.Remove(question);
        }

        await UniTask.WhenAll(tasks);
        
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

    private int _point = 0;
    private int _stepPoint = 0;

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
        
        for(int i=0; i<trouts.Length; i++)
        {
            for(int j=0; j < trouts[i].Length; j++)
            {
                switch (trouts[i][j])
                {
                    case EvolutionaryType.Egg:
                        _point++;
                        _stepPoint++;
                        break;
                    case EvolutionaryType.Tadpole:
                        _point+=2;
                        _stepPoint++;
                        break;
                    case EvolutionaryType.Frog:
                        _point+=3;
                        _stepPoint++;
                        break;
                    default:
                        break;
                }
            }
        }

        if(_stepPoint > 0)
        {
            _stepPoint--; 
        }
    }

    /// <summary>
    /// ポイントを取得
    /// </summary>
    /// <param name="frog"> カエルオブジェクト </param>
    /// <returns></returns>
    public int GetPoint(List<Frog> frogList)
    {
        foreach(var frog in frogList)
        {
            if(frog.IsCorrect.Value)
            {
                _point += 5;
            }

            frog.ResetEvolveCount();
        }

        return _point;
    }

    private List<int[]> _correctIndexList = new List<int[]>();

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

        _correctIndexList.Clear();
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
                _correctIndexList.Clear();
                return false;
            }

            _correctIndexList.Add(new int[] { i + startIndex, i });
        }

        return true;
    }

    /// <summary>
    /// カエルオブジェクトに正解を設定
    /// </summary>
    /// <param name="trouts"></param>
    private void SetCorrect(ReactiveProperty<Frog>[][] trouts)
    {
        foreach(var index in _correctIndexList)
        {
            trouts[index[0]][index[1]].Value.SetCorrect(true);
        }
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