using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ビューベース
/// </summary>
public abstract class ViewBase : UIBase
{
    /// <summary>
    /// UIを表示
    /// </summary>
    /// <param name="animationTime"> アニメーションの時間 </param>
    /// <returns></returns>
    public abstract UniTask ShowAsync(CancellationToken ct);

    /// <summary>
    /// UIを消す
    /// </summary>
    /// <param name="animationTime"> アニメーションの時間 </param>
    /// <returns></returns>
    public abstract UniTask HideAsync(CancellationToken ct);
}
