using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class QuestionPanelView : PanelViewBase
{
    [Header("アイコンのリスト")]
    [SerializeField]
    private List<Icon> _icons = new List<Icon>();
    [Header("お題の消えるときの追加の位置")]
    [SerializeField]
    private float _questionPosX = 300;

    protected override void Init()
    {
    }

    public async UniTask ShowAsync(float posY, CancellationToken ct)
    {
        await RectTransform
            .DOAnchorPosY(posY, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        await RectTransform
            .DOAnchorPosX(RectTransform.anchoredPosition.x + _questionPosX, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: ct);
    }

    /// <summary>
    /// アイコンを設定
    /// </summary>
    /// <param name="type"> 進化の状態 </param>
    /// <returns></returns>
    public void SetIcon(QuestionPanelCellPresenter cell, EvolutionaryType type)
    {
        if(type == EvolutionaryType.None)
        {
            return;
        }

        foreach(var icon in _icons)
        {
            if (icon.Type == type)
            {
                cell.SetIcon(icon.Sprite);
                return;
            }
        }
        
        Debug.LogError("アイコンが見つかりませんでした。");
    }
}

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
