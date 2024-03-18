using Cysharp.Threading.Tasks;
using System.Threading;

public interface IPresenter
{
    /// <summary>
    /// UIを表示
    /// </summary>
    /// <returns></returns>
    public UniTask ShowAsync(CancellationToken ct);

    /// <summary>
    /// UIを消す
    /// </summary>
    /// <returns></returns>
    public UniTask HideAsync(CancellationToken ct);
}
