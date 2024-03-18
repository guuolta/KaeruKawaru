using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPanelCellPresenter : PresenterBase<QuestionPanelCellView>
{
    public void SetIcon(Sprite icon)
    {
        View.IconImage.sprite = icon;
    }
}