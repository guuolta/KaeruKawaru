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

    // お題の表示数
    private int _questionMaxCount;
    // お題のUIサイズ
    private float _size;
    // 一番上のお題のY座標
    private float _topPosY;
    // お題の初期Y座標
    private float _panelIniPosY;
    // お題の位置
    private List<float> _panelPosList = new List<float>();
    // 表示しているお題
    private ReactiveCollection<QuestionPanelPresenter> _setPanelList = new ReactiveCollection<QuestionPanelPresenter>();


    /// <summary>
    /// お題パネルの初期設定
    /// </summary>
    /// <param name="questionMaxCount"> お題の最大数 </param>
    public void SetInit(int questionMaxCount)
    {
        _questionMaxCount = questionMaxCount;

        // お題のサイズ(縦横の小さい方に合わせた正方形)
        float sizeX = RectTransform.rect.width - _margin * 2;
        float sizeY = (RectTransform.rect.height - (_margin * 2 + _padding * (questionMaxCount - 1))) / questionMaxCount;
        _size = Mathf.Min(sizeX, sizeY);

        // お題とお題の間隔
        float interval = _padding + _size;
        
        // お題の位置
        _topPosY = RectTransform.rect.height / 2 - _margin - _size / 2;
        for (int i = 0; i < questionMaxCount; i++)
        {
            _panelPosList.Add(_topPosY - interval * i);
        }

        // 最初のお題の位置
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
        // 表示するお題が追加されたらパネルの位置を古いもの順に位置調整
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

        // お題が消えたら消すお題を非表示にする
        _setPanelList
            .ObserveRemove()
            .TakeUntilDestroy(this)
            .Subscribe(value =>
            {
                value.Value.HideAsync(ct).Forget();
            });

        // 初期のパネルを表示
        foreach (var panel in _setPanelList)
        {
            panel.ShowAsync(_panelPosList[_setPanelList.IndexOf(panel)], ct).Forget();
        }
    }

    /// <summary>
    /// パネルを追加
    /// </summary>
    /// <param name="panel"></param>
    /// <returns></returns>
    public void AddPanel(QuestionPanelPresenter panel)
    {
        if(_setPanelList.Count >= _questionMaxCount)
        {
            return;
        }
        
        // 初期位置に設定
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

    /// <summary>
    /// 使用禁止
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override UniTask ShowAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 使用禁止
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override UniTask HideAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }
}
