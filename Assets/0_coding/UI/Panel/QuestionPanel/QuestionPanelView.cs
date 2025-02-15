using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class QuestionPanelView : ViewBase
{
    [Header("アイコンのリスト")]
    [SerializeField]
    private List<Icon> _icons = new List<Icon>();
    [Header("お題の消えるときの追加の位置")]
    [SerializeField]
    private float _questionPosX = 300;

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="posY">動かす位置</param>
    /// <param name="ct"></param>
    public async UniTask ShowAsync(float posY, CancellationToken ct)
    {
        // すでに目的地なら終了
        if(RectTransform.anchoredPosition.y == posY)
        {
            return;
        }

        RectTransform.DOComplete();
        // 縦移動
        await RectTransform
            .DOAnchorPosY(posY, AnimationSec)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: ct);
    }
    
    public async UniTask HideAsync(CancellationToken ct)
    {
        RectTransform.DOComplete();
        // 画面外に横移動
        await RectTransform
            .DOAnchorPosX(RectTransform.anchoredPosition.x + _questionPosX, AnimationSec)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: ct);
    }

    /// <summary>
    /// アイコンを設定
    /// </summary>
    /// <param name="type"> 進化の状態 </param>
    /// <returns></returns>
    public void SetIcon(QuestionPanelCellPresenter cellPanel, EvolutionaryType type)
    {
        if(type == EvolutionaryType.None)
        {
            return;
        }

        // マスにアイコンを設定
        foreach(var icon in _icons)
        {
            if (icon.Type == type)
            {
                cellPanel.SetIcon(icon.Sprite);
                return;
            }
        }
        
        Debug.LogError("アイコンが見つかりませんでした。");
    }
}

/// <summary>
/// アイコンのデータ
/// </summary>
[System.Serializable]
public class Icon
{
    [Header("アイコンの種類")]
    [SerializeField]
    private EvolutionaryType _type;
    /// <summary>
    /// アイコンの種類
    /// </summary>
    public EvolutionaryType Type => _type;
    [Header("アイコンの画像")]
    [SerializeField]
    private Sprite _sprite;
    /// <summary>
    /// アイコンの画像
    /// </summary>
    public Sprite Sprite => _sprite;
}
