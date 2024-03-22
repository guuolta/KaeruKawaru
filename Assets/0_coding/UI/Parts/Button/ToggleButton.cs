using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : ButtonBase
{
    [Header("Onの時の画像")]
    [SerializeField]
    private Image _onImage;
    [Header("Offの時の画像")]
    [SerializeField]
    private Image _offImage;

    private BoolReactiveProperty _isOn = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsOn => _isOn;

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventClick();
        SetEventIsOn();
    }

    /// <summary>
    /// クリックしたときのイベント設定
    /// </summary>
    private void SetEventClick()
    {
        OnClickCallback += () =>
        {
            _isOn.Value = !_isOn.Value;
        };
    }
    
    /// <summary>
    /// Onの時の画像とOffの時の画像を設定する
    /// </summary>
    private void SetEventIsOn()
    {
        _isOn
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                _onImage.gameObject.SetActive(value);
                _offImage.gameObject.SetActive(!value);
            });
    }
}