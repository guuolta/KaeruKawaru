using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// セレクトメニューの音量調節パネル
/// </summary>
public class SelectSoundPanelPresenter : SoundPanelPresenterBase
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetButton(Ct);
    }

    /// <summary>
    /// ステージセレクトでパネルを開く
    /// </summary>
    /// <param name="ct"></param>
    private void SetButton(CancellationToken ct)
    {
        View.CloseButton.OnClickCallback += () => {
            SelectPanelManager.Instance.OpenPanelAsync(SelectPanelType.Slect,ct).Forget();
        };
    }
}
