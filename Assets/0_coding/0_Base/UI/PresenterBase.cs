using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// プレゼンターベース
/// </summary>
/// <typeparam name="TView"> ビュー </typeparam>
public class PresenterBase<TView> : UIBehaviour
    where TView : ViewBase
{
    [SerializeField, HideInInspector]
    private TView _view;
    /// <summary>
    /// ビュー
    /// </summary>
    protected TView View => _view;

    #if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        _view ??= GetComponent<TView>();
    }
    #endif
    
    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Init()
    {
        View.Init();
        SetEvent();
    }

    /// <summary>
    /// イベント発行
    /// </summary>
    protected virtual void SetEvent()
    {
        
    }
}
