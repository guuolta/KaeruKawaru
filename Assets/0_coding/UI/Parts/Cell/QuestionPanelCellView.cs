using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPanelCellView : ViewBase
{
    [SerializeField, HideInInspector]
    private Image _iconImage;
    
    #if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        _iconImage ??= Transform.GetChild(0).GetComponent<Image>();
    }
    #endif

    /// <summary>
    /// アイコンを設定
    /// </summary>
    /// <param name="icon">アイコンの画像</param>
    public void SetIcon(Sprite icon)
    {
        _iconImage.sprite = icon;
    }
}