using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CheckBoxBase : AnimationPartBase
{
    private BoolReactiveProperty _isCheck = new BoolReactiveProperty(false);
    /// <summary>
    /// チェックボックスのOn、Off
    /// </summary>
    public BoolReactiveProperty IsCheck => _isCheck;
    [Header("チェックボックスのマスク画像")]
    [SerializeField]
    private Image _mask;

    protected override void SetEvent()
    {
        SetEventClick();
        SetEventCheckBox();
    }

    private void SetEventClick()
    {
        OnClickCallback += () =>
        {
            _isCheck.Value = !_isCheck.Value;
            //AudioManager.Instance.PlayOneShotSE(_isCheck.Value ? SEType.Posi : SEType.Nega);
        };
    }

    public void SetEventCheckBox()
    {
        IsCheck
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if(value)
                {
                    _mask.gameObject.SetActive(false);
                }
                else
                {
                    _mask.gameObject.SetActive(true);
                }
            });
    }

    public void SetValue(bool value)
    {
        _isCheck.Value = value;
    }
}
