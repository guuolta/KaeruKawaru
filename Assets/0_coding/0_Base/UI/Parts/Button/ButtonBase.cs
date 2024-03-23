using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBase : AnimationPartBase
{
    [Header("押す前の画像")]
    [SerializeField]
    private Sprite _normalImage;
    [Header("押した後の画像")]
    [SerializeField]
    private Sprite _pushedImage;
    [Header("SE")]
    [SerializeField]
    private SEType _seType = SEType.Posi;

    private Image _image;
    protected Image Image
    {
        get
        {
            if (_image == null)
                _image = GetComponent<Image>();

            return _image;
        }
    }

    protected override void Init()
    {
        base.Init();
        ChangeInteractive(true);
    }

    protected override void SetEvent()
    {
        SetEventPlaySe();
        SetEventDobleClickPrevention();
    }

    /// <summary>
    /// SEを鳴らす
    /// </summary>
    protected void SetEventPlaySe()
    {
        if (_seType == SEType.None)
            return;

        OnClickCallback += () =>
        {
            AudioManager.Instance.PlayOneShotSE(_seType);
        };
    }

    /// <summary>
    /// 連打防止
    /// </summary>
    protected virtual void SetEventDobleClickPrevention()
    {
        OnClickCallback += async () =>
        {
            ChangeInteractive(false);
            await UniTask.WaitForSeconds(0.1f, cancellationToken: Ct);
            ChangeInteractive(true);
        };
    }

    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (_pushedImage!=null)
            Image.sprite = _pushedImage;
    }

    public override void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (_pushedImage!=null)
            Image.sprite = _normalImage;
    }
}
