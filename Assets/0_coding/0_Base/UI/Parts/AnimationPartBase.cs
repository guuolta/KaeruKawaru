using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

/// <summary>
/// 基本的なUIのパーツのアニメーションを共通化
/// </summary>
public class AnimationPartBase : UIBase,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    private Subject<Unit> _onClickEvent = new Subject<Unit>();
    /// <summary>
    /// クリック時に実行する処理
    /// </summary>
    public IObservable<Unit> OnClickEvent => _onClickEvent;
    
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // 縮小
        DoScaleAsync(0.8f, Ease.InSine).Forget();
        // 半透明にする
        DoFadeAsync(0.8f, Ease.InSine).Forget();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        // 元の大きさに戻す
        DoScaleAsync(1f, Ease.OutSine).Forget();
        // 元の透明度にする
        DoFadeAsync(1, Ease.OutSine).Forget();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        // 拡大
        DoScaleAsync(1.05f, Ease.InSine).Forget();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        // 元の大きさにする
        DoScaleAsync(1f, Ease.OutSine).Forget();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onClickEvent.OnNext(Unit.Default);
    }
}