using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClipButton : ButtonBase
{
    [Header("待機時間")]
    [SerializeField]
    private float _waitTime = 1f;
    [Header("チェックマーク")]
    [SerializeField]
    private Sprite _check;
    [Header("説明テキスト")]
    [SerializeField]
    private CanvasGroup _descriptionGroup;
    [Header("初期説明")]
    [SerializeField]
    private string _defDescription = "URLをクリップボードにコピー";

    private Sprite _defSprite;
    private Image _image;
    private TMP_Text _descriptionText;

    protected override void Init()
    {
        base.Init();
        _image = GetComponent<Image>();
        _defSprite = _image.sprite;
        _descriptionText = _descriptionGroup.gameObject.GetComponentInChildren<TMP_Text>();
        _descriptionText.text = _defDescription;
        Hide(_descriptionGroup);
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventClick();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        ShowAsync(_descriptionGroup, Ct).Forget();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        HideAsync(_descriptionGroup, Ct).Forget();
    }

    private void SetEventClick()
    {
        OnClickCallback += async () =>
        {
            ChangeInteractive(false);
            _image.sprite = _check;
            await UniTask.WaitForSeconds(_waitTime);
            _image.sprite = _defSprite;
            ChangeInteractive(true);
        };
    }
}
