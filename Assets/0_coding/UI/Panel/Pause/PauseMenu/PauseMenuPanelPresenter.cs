using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class PauseMenuPanelPresenter : PanelPresenterBase<PauseMenuPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventButton(Ct);
    }

    /// <summary>
    /// ボタンのイベント設定
    /// </summary>
    private void SetEventButton(CancellationToken ct)
    {
        // ゲームに戻る
        View.ReturnButton.OnClickCallback += () =>
        {
            PausePanelManager.Instance.ClosePanelAsync(ct).Forget();
        };
        
        // 音量調節
        View.SoundSettingButton.OnClickCallback += () =>
        {
            PausePanelManager.Instance.OpenPanelAsync(PausePanelType.Sound, ct).Forget();
        };
        
        // リトライ
        View.RetryButton.OnClickCallback += () =>
        {
            GameSceneManager.ReLoadSceneAsync().Forget();
        };
        
        // タイトルへ
        View.TitleButton.OnClickCallback += () =>
        {
            PausePanelManager.Instance.OpenPanelAsync(PausePanelType.Confirm, ct).Forget();
        };
    }
}
