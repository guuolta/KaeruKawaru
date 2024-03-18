using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;

/// <summary>
/// プレゼンターベース
/// </summary>
/// <typeparam name="TView"> ビュー </typeparam>
public class PresenterBase<TView> : ObjectBase, IPresenter
    where TView : ViewBase
{
    private TView _view;
    /// <summary>
    /// ビュー
    /// </summary>
    protected TView View
    {
        get
        {
            if (_view == null)
            {
                _view = GetComponent<TView>();
            }

            return _view;
        }
    }
    private BoolReactiveProperty _isOpen = new BoolReactiveProperty(false);
    /// <summary>
    /// パネルを開いているか
    /// </summary>
    public BoolReactiveProperty IsOpen => _isOpen;

    public virtual async UniTask ShowAsync(CancellationToken ct)
    {
        await View.ShowAsync(ct);
        _isOpen.Value = true;
        View.ChangeInteractive(true);
    }

    public virtual async UniTask HideAsync(CancellationToken ct)
    {
        View.ChangeInteractive(false);
        _isOpen.Value = false;
        await View.HideAsync(ct);
    }
}
