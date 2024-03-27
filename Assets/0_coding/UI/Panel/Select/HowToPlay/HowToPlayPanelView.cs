using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        _slidePanelList[_index.Value].ChangeInteractive(true);
        _slidePanelList[_index.Value].RectTransform.anchoredPosition = new Vector2(_showposX,_inipos.y);
    }
    protected override void SetEvent()
    {
        base.SetEvent();
        SetButton();
    }
    private void SetList()
    {
        for(int i=0;i<Slideparent.childCount;i++)
        {
            var slide = Slideparent.GetChild(i).GetComponent<SlidePanel>();
            _slidePanelList.Add(slide);
            slide.SetInipos(_inipos);
        }
        _listCount = _slidePanelList.Count;
    }
    public async UniTask SlideLeftAsync(CancellationToken ct)
    {
        await _slidePanelList[_index.Value].HideAsync(_hideleftposX,ct);
        ChangeIndex(_index.Value - 1);
        await _slidePanelList[_index.Value].ShowAsync(_showposX,ct);
    }
    public async UniTask SlideRightAsync(CancellationToken ct)
    {
        await _slidePanelList[_index.Value].HideAsync(_hiderightposX,ct);
        ChangeIndex(_index.Value + 1);
        await _slidePanelList[_index.Value].ShowAsync(_showposX,ct);
    }
    private void ChangeIndex(int value)
    {
        _index.Value = Mathf.Clamp(value,0,_listCount-1);
    }
    private void SetButton()
    {
        _index
            .TakeUntilDestroy(this)
            .Subscribe(value => {
                LeftButton.SetisHide(value > 0);
                RightButton.SetisHide(value < _listCount-1);
            });
    }
}
