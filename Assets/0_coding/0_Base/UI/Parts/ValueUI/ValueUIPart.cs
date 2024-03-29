using UniRx;
using UnityEngine;

/// <summary>
/// スライダーとインプットフィールドをつなげる
/// </summary>
[System.Serializable]
public class ValueUIPart : UIBase
{
    [Header("スライダーとインプットフィールドの最小値")]
    [SerializeField]
    private int _minValue;
    [Header("スライダーとインプットフィールドの最大値")]
    [SerializeField]
    private int _maxValue;
    [Header("対象のスライダー")]
    [SerializeField]
    private SliderBase _slider;
    [Header("対象のインプットフィールド")]
    [SerializeField]
    private ValueInputFieldBase _inputField;
    private ReactiveProperty<int> _value = new ReactiveProperty<int>();
    /// <summary>
    /// 値
    /// </summary>
    public ReactiveProperty<int> Value => _value;

    protected override void SetFirstEvent()
    {
        _slider.SetSlider(_minValue, _maxValue);
        _inputField.SetInputField(_minValue, _maxValue);
        SetEventChangeValue();
    }

    /// <summary>
    /// スライダーとインプットフィールドの値のイベント
    /// </summary>
    private void SetEventChangeValue()
    {
        _slider.SliderValueAsObservable
            .TakeUntilDestroy(this)
            .Skip(1)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                _inputField.SetValue(value);
                _value.Value = (int)value;
            });

        _inputField.InputValueAsObservable
            .Skip(1)
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                _slider.SetValue(value);
                _value.Value = (int)value;
            });
    }

    /// <summary>
    /// 値を設定する
    /// </summary>
    /// <param name="value"> 値 </param>
    public void SetValue(int value)
    {
        _slider.SetValue(value);
        _inputField.SetValue(value);
        _value.Value = value;
    }
}
