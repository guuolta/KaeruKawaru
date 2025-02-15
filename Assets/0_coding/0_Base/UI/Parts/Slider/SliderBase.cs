using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スライダーのベース
/// </summary>
public class SliderBase : UIBase
{
    [SerializeField, HideInInspector]
    private Slider _slider;
    /// <summary>
    /// スライダー
    /// </summary>
    public Slider Slider => _slider;
    
    private IObservable<float> _sliderValueAsObservable;
    /// <summary>
    /// スライダーの値
    /// </summary>
    public IObservable<float> SliderValueAsObservable
    {
        get
        {
            if( _sliderValueAsObservable == null)
            {
                _sliderValueAsObservable = Slider.OnValueChangedAsObservable();
            }

            return _sliderValueAsObservable;
        }
    }

    #if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        _slider ??= GetComponent<Slider>();
    }
    #endif

    /// <summary>
    /// スライダーの初期設定
    /// </summary>
    /// <param name="minValue">最小値</param>
    /// <param name="maxValue">最大値</param>
    public void InitSlider(float minValue, float maxValue)
    {
        Slider.minValue = minValue;
        Slider.maxValue = maxValue;
    }

    /// <summary>
    /// 値を設定
    /// </summary>
    /// <param name="value">設定する値</param>
    public void SetValue(float value)
    {
        Slider.value = value;
    }
}