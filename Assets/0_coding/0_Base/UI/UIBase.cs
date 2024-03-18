using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// UI系のベース
/// </summary>
public class UIBase : GameObjectBase,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("押せないときの透明度")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _disInteractiveAlpha = 0.8f;
    /// <summary>
    /// アニメーションの時間
    /// </summary>
    [Header("アニメーションの時間")]
    [Range(0f, 10f)]
    [SerializeField]
    protected float AnimationTime = 0.1f;

    private RectTransform _rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if(_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            return _rectTransform;
        }
    }

    private CanvasGroup _canvasGroup;
    public CanvasGroup CanvasGroup
    {
        get
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
                if (_canvasGroup == null)
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            return _canvasGroup;
        }
    }

    /// <summary>
    /// UIが押されたときのイベント
    /// </summary>
    public System.Action OnClickCallback;

    /// <summary>
    /// UIが押されたとき処理を実行
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickCallback?.Invoke();
    }

    /// <summary>
    /// UIが押し込まれたときの処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    /// <summary>
    /// UIから押し離されたときの処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerUp(PointerEventData eventData)
    {

    }

    /// <summary>
    /// UIにカーソルが入った時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {

    }

    /// <summary>
    /// UIからカーソルが離れた時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerExit(PointerEventData eventData)
    {

    }

    /// <summary>
    /// UIを押せるようにするか設定
    /// </summary>
    /// <param name="isInteractive">押せるか</param>
    public void ChangeInteractive(bool isInteractive)
    {
        CanvasGroup.interactable = isInteractive;
        CanvasGroup.blocksRaycasts = isInteractive;
        if (isInteractive)
        {
            CanvasGroup.alpha = 1f;
        }
        else
        {
            CanvasGroup.alpha = _disInteractiveAlpha;
        }
    }

    /// <summary>
    /// 画像を表示
    /// </summary>
    /// <param name="image"></param>
    public virtual void Show(Image image)
    {
        Color newColor = image.color;
        newColor.a = 1;
        image.color = newColor;
    }
    
    /// <summary>
    /// UI表示
    /// </summary>
    /// <param name="canvasGroup"></param>
    public virtual void Show(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
    }

    /// <summary>
    /// 画像を消す
    /// </summary>
    /// <param name="image"></param>
    public virtual void Hide(Image image)
    {
        Color newColor = image.color;
        newColor.a = 0;
        image.color = newColor;
    }

    /// <summary>
    /// UIを消す
    /// </summary>
    /// <param name="canvasGroup"></param>
    public virtual void Hide(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
    }

    /// <summary>
    /// 画像をアニメーションで表示
    /// </summary>
    /// <param name="image">画像</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async UniTask ShowAsync(Image image, CancellationToken ct)
    {
        image.DOComplete();
        if (image.color.a == 1)
        {
            return;
        }

        await image.DOFade(1, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: ct);
    }

    /// <summary>
    /// UIをアニメーショで表示
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async UniTask ShowAsync(CanvasGroup canvasGroup, CancellationToken ct)
    {
        canvasGroup.DOComplete();
        if (canvasGroup.alpha == 1)
        {
            return;
        }

        await canvasGroup.DOFade(1, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: ct);
        ChangeInteractive(true);
    }

    /// <summary>
    /// 画像をアニメーショで消す
    /// </summary>
    /// <param name="image"> 画像 </param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async UniTask HideAsync(Image image, CancellationToken ct)
    {
        image.DOComplete();
        if (image.color.a == 0)
        {
            return;
        }

        await image.DOFade(0, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: ct);
    }

    /// <summary>
    /// UIをアニメーショで消す
    /// </summary>
    /// <param name="canvasGroup"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public virtual async UniTask HideAsync(CanvasGroup canvasGroup, CancellationToken ct)
    {
        canvasGroup.DOComplete();
        if (canvasGroup.alpha == 0)
        {
            return;
        }

        await canvasGroup.DOFade(0, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: ct);

        ChangeInteractive(false);
    }
}
