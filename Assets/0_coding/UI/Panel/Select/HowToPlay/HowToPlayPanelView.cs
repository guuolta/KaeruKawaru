using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class HowToPlayPanelView : SelectPanelViewBase
{
    [Header("スライドの親オブジェクト")]
    [SerializeField]
    private Transform Slideparent;

    [Header("左ボタン")]
    [SerializeField]
    private ArrowButton _leftButton;
    public ArrowButton LeftButton => _leftButton;

    [Header("右ボタン")]
    [SerializeField]
    private ArrowButton _rightButton;
    public ArrowButton RightButton => _rightButton;

    [Header("スライドの初期位置")]
    [SerializeField]
    private Vector2 _inipos;

    [Header("スライドの表示位置")]
    [SerializeField]
    private float _showposX;

    [Header("スライドを左に移動させた位置")]
    [SerializeField]
    private float _hideleftposX;

    [Header("スライドを右に移動させた位置")]
    [SerializeField]
    private float _hiderightposX;

    private ReactiveProperty<int> _index = new ReactiveProperty<int>();
    private int _listCount;

    private List<SlidePanel> _slidePanelList = new List<SlidePanel>();

    protected override void Init()
    {
        base.Init();
        SetList();
        SetIniPos(Ct);
    }

    /// <summary>
    /// 表示するスライドのリストを設定
    /// </summary>
    private void SetList()
    {
        for (int i = 0; i < Slideparent.childCount; i++)
        {
            var slide = Slideparent.GetChild(i).GetComponent<SlidePanel>();
            _slidePanelList.Add(slide);
            slide.SetInit(_inipos);
        }
        _listCount = _slidePanelList.Count;
    }

    /// <summary>
    /// 初期位置を設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetIniPos(CancellationToken ct)
    {
        //1スライド目が正面
        _slidePanelList[0].RectTransform.anchoredPosition = new Vector2(_showposX, _inipos.y);
        _slidePanelList[0].ChangeInteractive(true);
        
        // 2枚目以降は右に非表示で配置
        for (int i=1;i< _slidePanelList.Count;i++)
        {
            _slidePanelList[i].ChangeInteractive(false);
            _slidePanelList[i].RectTransform.anchoredPosition = new Vector2(_hiderightposX, _inipos.y);
        }

        _index.Value = 0;
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventButton();
    }
    
    /// <summary>
    /// ページを戻す
    /// </summary>
    /// <param name="ct"></param>
    public async UniTask SlideLeftAsync(CancellationToken ct)
    {
        // 現在のスライドを左に移動し次のスライドを右から真ん中に移動させる
        await _slidePanelList[_index.Value].HideAsync(_hiderightposX, ct);
        ChangeIndex(_index.Value - 1);
        await _slidePanelList[_index.Value].ShowAsync(_showposX,ct);
    }
    
    /// <summary>
    /// スライドを進める
    /// </summary>
    /// <param name="ct"></param>
    public async UniTask SlideRightAsync(CancellationToken ct)
    {
        // 現在のスライドを右に移動し次のスライドを左から真ん中に移動させる
        await _slidePanelList[_index.Value].HideAsync(_hideleftposX, ct);
        ChangeIndex(_index.Value + 1);
        await _slidePanelList[_index.Value].ShowAsync(_showposX,ct);
    }
    
    /// <summary>
    /// 現在のスライド番号を更新
    /// </summary>
    /// <param name="value"></param>
    private void ChangeIndex(int value)
    {
        _index.Value = Mathf.Clamp(value,0,_listCount-1);
    }
    
    /// <summary>
    /// ボタンのイベント
    /// </summary>
    private void SetEventButton()
    {
        // スライド晩語が端に来たら、それ以上めくれないようにボタンを非表示
        _index
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value => {
                LeftButton.SetIsHide(value > 0);
                RightButton.SetIsHide(value < _listCount-1);
            });
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        await base.HideAsync(ct);
        // 初期状態に戻す
        SetIniPos(Ct);
    }
}
