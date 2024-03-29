using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

/// <summary>
/// 基底クラス
/// </summary>
public class ObjectBase : MonoBehaviour
{
    private CancellationToken _ct;
    /// <summary>
    /// キャンセレーショントークン
    /// </summary>
    public CancellationToken Ct
    {
        get
        {
            if (_ct == null)
                _ct = this.GetCancellationTokenOnDestroy();

            return _ct;
        }
    }
    private List<Tween> _tweenList = new List<Tween>();

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        SetFirstEvent();
        SetEvent();
    }

    private void OnDestroy()
    {
        Destroy();
    }

    /// <summary>
    /// 変数の初期化など
    /// </summary>
    protected virtual void Init()
    {

    }

    /// <summary>
    /// 先に行いたいイベントの発行
    /// </summary>
    protected virtual void SetFirstEvent()
    {

    }

    /// <summary>
    /// イベントの発行
    /// </summary>
    protected virtual void SetEvent()
    {

    }

    /// <summary>
    /// インスタンス破壊時にする処理
    /// </summary>
    protected virtual void Destroy()
    {
        DisposeTween();
    }

    /// <summary>
    /// DOTweenのアニメーションを破棄
    /// </summary>
    protected virtual void DisposeTween()
    {
        foreach (var tween in _tweenList)
        {
            tween.Kill();
        }

        _tweenList.Clear();
    }

    /// <summary>
    /// イベント削除
    /// </summary>
    protected virtual CompositeDisposable DisposeEvent(CompositeDisposable disposable)
    {
        disposable.Dispose();
        return new CompositeDisposable();
    }

    /// <summary>
    /// DOTweenのアニメーションを追加
    /// </summary>
    /// <param name="tween"></param>
    public void AddTween(Tween tween)
    {
        _tweenList.Add(tween);
    }
}
