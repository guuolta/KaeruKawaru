using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class QuestionGroupView : ViewBase
{
    [Header("余白")]
    [SerializeField]
    private float _margin;
    [Header("お題パネルの間隔")]
    [SerializeField]
    private float _padding;

    private bool _isRemoving = false;
    private int _questionMaxCount;
    private float _size;
    private float _topPos;
    private float _panelIniPosY;
    private List<float> _panelPosList = new List<float>();
    private ReactiveCollection<QuestionPanelPresenter> _setPanelList = new ReactiveCollection<QuestionPanelPresenter>();


    /// <summary>
    /// お題パネルの初期設定
    /// </summary>
    /// <param name="questionMaxCount"> お題の最大数 </param>
    public void SetInit(int questionMaxCount)
    {
        _questionMaxCount = questionMaxCount;

        float sizeX = RectTransform.rect.width - _margin * 2;
        float sizeY = (RectTransform.rect.height - (_margin * 2 + _padding * (questionMaxCount - 1))) / questionMaxCount;
        _size = Mathf.Min(sizeX, sizeY);

        float interval = _padding + _size;
        _topPos = RectTransform.rect.height / 2 - _margin - _size / 2;

        for (int i = 0; i < questionMaxCount; i++)
        {
            _panelPosList.Add(_topPos - interval * i);
        }

        _panelIniPosY = -(RectTransform.rect.height / 2 + _size);
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventPanelList(Ct);
    }

    /// <summary>
    /// パネルのイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventPanelList(CancellationToken ct)
    {
        _setPanelList
            .ObserveAdd()
            .TakeUntilDestroy(this)
            .Subscribe(list =>
            {
                foreach (var panel in _setPanelList)
                {
                    panel.ShowAsync(_panelPosList[_setPanelList.IndexOf(panel)], ct).Forget();
                }
            });

        _setPanelList
            .ObserveRemove()
            .TakeUntilDestroy(this)
            .Subscribe(value =>
            {
                value.Value.HideAsync(ct).Forget();
            });

        foreach (var panel in _setPanelList)
        {
            panel.ShowAsync(_panelPosList[_setPanelList.IndexOf(panel)], ct).Forget();
        }
    }

    /// <summary>
    /// パネルを設定
    /// </summary>
    /// <param name="panel"></param>
    /// <returns></returns>
    public void SetPanel(QuestionPanelPresenter panel)
    {
        if(_setPanelList.Count >= _questionMaxCount)
        {
            return;
        }

        panel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, _panelIniPosY, 0);
        _setPanelList.Add(panel);
    }

    /// <summary>
    /// パネルを削除
    /// </summary>
    /// <param name="panel"></param>
    /// <returns></returns>
    public void RemovePanel(QuestionPanelPresenter panel)
    {
        _setPanelList.Remove(panel);
    }

    public override UniTask ShowAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }

    public override UniTask HideAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }
}
