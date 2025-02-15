using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonBase : AnimationPartBase
{
    [SerializeField, HideInInspector]
    private Image _image;
    public Image Image => _image;
    
    [Header("押す前の画像")]
    [SerializeField]
    private Sprite _normalImage;
    [Header("押した後の画像")]
    [SerializeField]
    private Sprite _pushedImage;
    [Header("SE")]
    [SerializeField]
    private SEType _seType = SEType.Posi;

    #if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        _image ??= GetComponent<Image>();
    }
    #endif

    protected override void SetEvent()
    {
        SetEventPlaySe();
        SetEventDobleClickPrevention();
    }

    /// <summary>
    /// クリック時にSEを鳴らす
    /// </summary>
    private void SetEventPlaySe()
    {
        if (_seType == SEType.None)
            return;

        OnClickEvent
            .Subscribe(_=>
            {
                AudioManager.Instance.PlayOneShotSE(_seType);
            });
    }

    /// <summary>
    /// 連打防止
    /// </summary>
    protected virtual void SetEventDobleClickPrevention()
    {
        OnClickEvent
            .Subscribe(async _ =>
            {
                ChangeInteractive(false);
                await UniTask.WaitForSeconds(0.1f, cancellationToken: destroyCancellationToken);
                ChangeInteractive(true);
            });
    }

    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (_pushedImage!=null) // 押しているときの画像にする
            Image.sprite = _pushedImage;
    }

    public override void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (_pushedImage!=null) // 通常の画像に戻す
            Image.sprite = _normalImage;
    }
}
