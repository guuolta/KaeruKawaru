using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPanelCellView : ViewBase
{
    private Image _iconImage;
    public Image IconImage
    {
        get
        {
            if(_iconImage == null)
                _iconImage = Transform.GetChild(0).GetComponent<Image>();

            return _iconImage;
        }
    }

    public override UniTask ShowAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }

    public override UniTask HideAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }
}