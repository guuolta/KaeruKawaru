using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBase : UIBase
{
    [Header("トグルON時の画像")]
    [SerializeField]
    private Image _onImage;
    [Header("トグルOFF時の画像")]
    [SerializeField]
    private Image _offImage;

    private BoolReactiveProperty _isToggle = new BoolReactiveProperty(false);
    /// <summary>
    /// トグルのOn、Off
    /// </summary>
    public BoolReactiveProperty IsToggle => _isToggle;
    private Slider _slider;
    protected Slider slider
    {
        get
        {
            if (_slider == null)
            {
                _slider = GetComponent<Slider>();
                _slider.interactable = false;
                _slider.minValue = 0;
                _slider.maxValue = 1;
                _slider.wholeNumbers = true;
                _slider.value = 0;
            }
            return _slider;
        }
    }

    [Header("ハンドル")]
    [SerializeField]
    private HandleBase _handle;
    protected HandleBase handle
    {
        get
        {
            if( _handle == null)
            {
                slider.handleRect.GetComponent<HandleBase>();

                if(_handle == null)
                    _handle = slider.handleRect.AddComponent<HandleBase>();
            }

            return _handle;
        }
    }

    protected override void Init()
    {
        _onImage.color -= new Color(0, 0, 0, 1);
        _offImage.color += new Color(0, 0, 0, 1);
    }

    protected override void SetEvent()
    {
        SetEventClick();
        SetEventCheckBox();
    }

    private void SetEventClick()
    {
        handle.OnClickCallback += () =>
        {
            _isToggle.Value = !_isToggle.Value;
        };
    }

    public void SetEventCheckBox()
    {
        IsToggle
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                if(value)
                {
                    slider
                        .DOValue(1, AnimationTime)
                        .SetEase(Ease.Linear)
                        .ToUniTask(cancellationToken: Ct)
                        .Forget();
                    _onImage
                        .DOFade(1, AnimationTime)
                        .SetEase(Ease.Linear)
                        .ToUniTask(cancellationToken: Ct)
                        .Forget();
                    await _offImage
                        .DOFade(0, AnimationTime)
                        .SetEase(Ease.OutSine)
                        .ToUniTask(cancellationToken: Ct);
                }
                else
                {
                    slider
                        .DOValue(0, AnimationTime)
                        .SetEase(Ease.Linear)
                        .ToUniTask(cancellationToken: Ct)
                        .Forget();
                    _onImage
                        .DOFade(0, AnimationTime)
                        .SetEase(Ease.Linear)
                        .ToUniTask(cancellationToken: Ct)
                        .Forget();
                    await _offImage
                        .DOFade(1, AnimationTime)
                        .SetEase(Ease.Linear)
                        .ToUniTask(cancellationToken: Ct);
                }
            });
    }

    public void SetValue(bool value)
    {
        _isToggle.Value = value;
    }
}
