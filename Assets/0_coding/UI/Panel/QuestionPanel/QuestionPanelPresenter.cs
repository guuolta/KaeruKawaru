using UnityEngine;
using UnityEngine.UI;

public class QuestionPanelPresenter : PanelPresenterBase<QuestionPanelView>
{
    [Header("マス")]
    [SerializeField]
    private QuestionPanelCellPresenter _cell;

    /// <summary>
    /// お題パネルを作成
    /// </summary>
    /// <param name="trouts"> お題 </param>
    public void CreateQuestionPanel(int[][] trouts)
    {
        var layoutGroup = GetComponent<GridLayoutGroup>();
        int rowCount = trouts.Length;
        int columnCount = trouts[0].Length;
        float size = Mathf.Min(View.RectTransform.sizeDelta.x / columnCount, View.RectTransform.sizeDelta.y / rowCount);
        layoutGroup.cellSize = new Vector2(size, size);
        View.RectTransform.sizeDelta = new Vector2(size * columnCount, size * rowCount);

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                var cell = Instantiate(_cell, transform);
                View.SetIcon(cell, (EvolutionaryType)trouts[i][j]);
            }
        }
    }
}
