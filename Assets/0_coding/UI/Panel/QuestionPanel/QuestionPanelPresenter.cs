using System.Globalization;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// お題のパネル
/// </summary>
public class QuestionPanelPresenter : PresenterBase<QuestionPanelView>
{
    [Header("マス")]
    [SerializeField]
    private QuestionPanelCellPresenter cellPanel;

    public async UniTask ShowAsync(float posY ,CancellationToken ct)
    {
        await View.ShowAsync(posY, ct);
    }

    public async UniTask HideAsync(CancellationToken ct)
    {
        await View.HideAsync(ct);
        View.GameObject.SetActive(false);
    }

    /// <summary>
    /// お題パネルを作成
    /// </summary>
    /// <param name="trouts"> お題 </param>
    public void CreateQuestionPanel(EvolutionaryType[][] trouts)
    {
        // レイアウトグループの設定
        var layoutGroup = GetComponent<GridLayoutGroup>();
        int rowCount = trouts.Length;
        int columnCount = trouts[0].Length;
        // マスの大きさを縦横の小さい方に合わせた正方形にする
        float size = Mathf.Min(View.RectTransform.sizeDelta.x / columnCount, View.RectTransform.sizeDelta.y / rowCount);
        layoutGroup.cellSize = new Vector2(size, size);
        // パネルの大きさ調整
        View.RectTransform.sizeDelta = new Vector2(size * columnCount, size * rowCount);
        
        // マスを作って対応するアイコンを設定
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                var cell = Instantiate(cellPanel, transform);
                cell.Init();
                View.SetIcon(cell, trouts[i][j]);
            }
        }
    }
}
