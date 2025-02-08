using System.Threading;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

/// <summary>
/// 上下に泳いでいるように揺れるオブジェクト
/// </summary>
public class SwayingGameObject : GameObjectBase
{
    [Header("揺れる周期")]
    [Range(0f, 10f)]
    [SerializeField]
    private float _period = 5f;
    [Header("揺れる幅")]
    [Range(0f, 5f)]
    [SerializeField]
    private float _amplitude = 0.2f;

    private float _iniY;
    private Tween _tween;

    protected override void Init()
    {
        base.Init();
        _iniY = transform.localPosition.y;
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        StartAnimation(Ct);
    }

    // アニメーションを開始
    private void StartAnimation(CancellationToken ct)
    {
        // 上下する最大の高さ
        float maxY = _iniY + _amplitude;
        
        // 上下を繰り返す
        _tween = Transform.DOLocalMoveY(maxY, _period / 2)
                .SetEase(Ease.OutFlash, 1)
                .SetLoops(-1, LoopType.Yoyo);
        
        AddTween(_tween);
        
        // アニメーションを再生
        _tween.ToUniTask(cancellationToken: ct)
            .Forget();
    }
}
