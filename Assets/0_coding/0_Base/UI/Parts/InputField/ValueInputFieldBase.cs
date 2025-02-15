using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 数値を設定するインプットフィールド
/// </summary>
public class ValueInputFieldBase : AnimationPartBase
{
    private float _minValue;
    private float _maxValue;
    
    private TMP_InputField _inputField;
    /// <summary>
    /// インプットフィールド
    /// </summary>
    public TMP_InputField InputField => _inputField;
    
    private ReactiveProperty<float> _inputValueAsObservable = new ReactiveProperty<float>();
    /// <summary>
    /// InputFieldの値
    /// </summary>
    public ReactiveProperty<float> InputValueAsObservable => _inputValueAsObservable;

    #if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        _inputField ??= GetComponent<TMP_InputField>();
    }
    #endif

    /// <summary>
    /// インプットフィールドの初期値設定
    /// </summary>
    /// <param name="minValue"> インプットフィールドの最小値 </param>
    /// <param name="maxValue"> インプットフィールドの最大値 </param>
    public void InitInputField(float minValue, float maxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
        
        SetEvent();
    }
    
    protected override void SetEvent()
    {
        SetEventInputValue();
        SetEventClick();
    }
    
    /// <summary>
    /// インプットフィールドの値を設定
    /// </summary>
    private void SetEventInputValue()
    {
        UnityAction<string> onChangeEvent = (value) =>
        {
            // 入力された数値を反映
            float volume;
            if (float.TryParse(value, out volume))
            {
                InputValueAsObservable.Value = Mathf.Clamp(volume, _minValue, _maxValue);
            }
            else //数字以外が入力された0にする
            {
                InputValueAsObservable.Value = 0;
            }
        };

        InputField.onValueChanged.AddListener(onChangeEvent);
    }
    
    /// <summary>
    /// クリックしたときのイベント設定
    /// </summary>
    private void SetEventClick()
    {
        // クリックしたらSEを鳴らす
        OnClickEvent
            .Subscribe(_ =>
            {
                AudioManager.Instance.PlayOneShotSE(SEType.Posi);
            });
    }

    /// <summary>
    /// インプットフィールドの値を設定
    /// </summary>
    /// <param name="value"> 値 </param>
    public void SetValue(float value)
    {
        InputField.text = value.ToString();
    }
}
