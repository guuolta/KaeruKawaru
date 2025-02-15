using UnityEngine;

public class QuestionPanelCellPresenter : PresenterBase<QuestionPanelCellView>
{
    /// <summary>
    /// アイコンを設定
    /// </summary>
    /// <param name="icon">アイコンの画像</param>
    public void SetIcon(Sprite icon)
    {
        View.SetIcon(icon);
    }
}