using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class TitleManager : SingletonObjectBase<TitleManager>
{
    [Header("アニメーションの時間")]
    [SerializeField]
    private float _animationTime;
    [Header("タイトルにターゲット時の位置")]
    [SerializeField]
    private Vector3 _titleTargetPos;
    [Header("タイトルにターゲット時の角度")]
    [SerializeField]
    private Vector3 _titleTargetRot;
    [Header("セレクトにターゲット時の位置")]
    [SerializeField]
    private Vector3 _selectTargetPos;
    [Header("セレクトにターゲット時の角度")]
    [SerializeField]
    private Vector3 _selectTargetRot;

    private Camera _camera;
    private CompositeDisposable _disposable = new CompositeDisposable();
    //protected override void SetEvent()
    //{
    //    base.SetEvent();

    //    AudioManager.Instance.PlayBGM(BGMType.Title);
    //    Observable.EveryUpdate()
    //        .TakeUntilDestroy(this)
    //        .Where(_ => Input.GetMouseButtonDown(0))
    //        .Subscribe(_ =>
    //        {
    //            GameSceneManager.LoadScene(SceneType.EasyGame);
    //        });

    //}

    protected override void Init()
    {
        base.Init();
        _camera = Camera.main;
        _camera.transform.position = _titleTargetPos;
        _camera.transform.eulerAngles = _titleTargetRot;
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        AudioManager.Instance.PlayBGM(BGMType.Title);
    }

    /// <summary>
    /// タイトルにターゲット
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask TargetTitleAsync(CancellationToken ct)
    {
        await TargetAsync(_titleTargetPos, _titleTargetRot, ct);
    }

    /// <summary>
    /// セレクトにターゲット
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask TargetSelectAsync(CancellationToken ct)
    {
        await TargetAsync(_selectTargetPos, _selectTargetRot, ct);
    }

    /// <summary>
    /// ターゲット
    /// </summary>
    /// <param name="pos"> 対象の位置 </param>
    /// <param name="rot"> 対象の角度 </param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask TargetAsync(Vector3 pos, Vector3 rot,CancellationToken ct)
    {
        _camera.transform.DOComplete();

        await UniTask.WhenAll(new List<UniTask>
        {
            MoveCamera(pos, ct),
            RotateCamera(rot, ct)
        });
    }

    /// <summary>
    /// カメラを動かす
    /// </summary>
    /// <param name="targetPos"> 目的地 </param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask MoveCamera(Vector3 targetPos, CancellationToken ct)
    {
        await _camera.transform
            .DOMove(targetPos, _animationTime)
            .SetEase(Ease.InOutSine)
            .ToUniTask(cancellationToken: ct);
    }

    /// <summary>
    /// カメラを回転させる
    /// </summary>
    /// <param name="targetRot"> 対象の角度 </param>
    /// <param name="ct"></param>
    /// <returns></returns>
    private async UniTask RotateCamera(Vector3 targetRot, CancellationToken ct)
    {
        await _camera.transform
            .DORotate(targetRot, _animationTime)
            .SetEase(Ease.InOutSine)
            .ToUniTask(cancellationToken: ct);
    }
}
