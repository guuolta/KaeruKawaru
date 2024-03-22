using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ConfirmPanelPresenter : PanelPresenterBase<ConfirmPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventButton(Ct);
    }

    /// <summary>
    /// ボタンのイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventButton(CancellationToken ct)
    {
        View.YesButton.OnClickCallback += () =>
        {
            Debug.Log("タイトルロード");
            //GameSceneManager.LoadScene(SceneType.Title);
        };

        View.NoButton.OnClickCallback += () =>
        {
            PausePanelManager.Instance.ClosePanelAsync(ct).Forget();
        };
    }
}
