using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPanelCellView : ViewBase
{
    private Image _iconImage;
    protected Image _IconImage
    {
        get
        {
            if(_iconImage == null)
                _iconImage = Transform.GetChild(0).GetComponent<Image>();

            return _iconImage;
        }
    }

    /// <summary>
    /// アイコンを設定
    /// </summary>
    /// <param name="icon">アイコンの画像</param>
    public void SetIcon(Sprite icon)
    {
        _IconImage.sprite = icon;
    }

    /// <summary>
    /// 使用禁止
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override UniTask ShowAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 使用禁止
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override UniTask HideAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }
}