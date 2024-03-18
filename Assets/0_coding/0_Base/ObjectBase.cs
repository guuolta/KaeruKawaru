using Cysharp.Threading.Tasks;
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
        FirstDestroy();
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
    /// インスタンス破壊時に最初にする処理
    /// </summary>
    protected virtual void FirstDestroy()
    {

    }

    /// <summary>
    /// インスタンス破壊時にする処理
    /// </summary>
    protected virtual void Destroy()
    {

    }

    /// <summary>
    /// イベント削除
    /// </summary>
    protected virtual CompositeDisposable DisposeEvent(CompositeDisposable disposable)
    {
        disposable.Dispose();
        return new CompositeDisposable();
    }
}
