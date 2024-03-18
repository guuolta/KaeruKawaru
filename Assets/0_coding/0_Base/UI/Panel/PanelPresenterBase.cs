using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// パネルのプレゼンター
/// </summary>
/// <typeparam name="TView"> パネルビュー </typeparam>
public class PanelPresenterBase<TView> : PresenterBase<TView>
    where TView : PanelViewBase
{
    
}
