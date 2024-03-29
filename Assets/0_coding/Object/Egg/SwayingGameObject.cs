using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

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

    private void StartAnimation(CancellationToken ct)
    {
        float maxY = _iniY + _amplitude;

        _tween = Transform.DOLocalMoveY(maxY, _period / 2)
                .SetEase(Ease.OutFlash, 1)
                .SetLoops(-1, LoopType.Yoyo);
        AddTween(_tween);
        _tween.ToUniTask(cancellationToken: ct)
            .Forget();
    }
}
