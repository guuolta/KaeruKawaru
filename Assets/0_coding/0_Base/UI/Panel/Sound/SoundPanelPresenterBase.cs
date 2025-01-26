using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

/// <summary>
/// 音量調節パネル
/// </summary>
public class SoundPanelPresenterBase : PanelPresenterBase<SoundPanelView>
{
    protected override void SetEvent()
    {
        InitializeVolume();
        SetEventValueUIPart();
        SetEventMuteButton();
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        InitializeVolume();
        await base.ShowAsync(ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        await base.HideAsync(ct);
        AudioManager.Instance.SaveVolume();
    }

    /// <summary>
    /// 初期音量設定
    /// </summary>
    private void InitializeVolume()
    {
        int[] volumes = AudioManager.Instance.GetSoundVolumes();

        foreach (var soundUI in View.SoundUIList)
        {
            soundUI.SoundUIPart.SetValue(volumes[(int)soundUI.AudioType]);
        }
    }

    /// <summary>
    /// UIパーツイベント設定
    /// </summary>
    private void SetEventValueUIPart()
    {
        foreach (var soundUI in View.SoundUIList)
        {
            // 音量調節のUIが変わったら、音量を更新
            soundUI.SoundUIPart.Value
                .TakeUntilDestroy(this)
                .DistinctUntilChanged()
                .Subscribe(value =>
                {
                    AudioManager.Instance.SetVolume(soundUI.AudioType, value);
                });
        }
    }

    /// <summary>
    /// ミュートボタンのイベント設定
    /// </summary>
    private void SetEventMuteButton()
    {
        foreach (var soundUI in View.SoundUIList)
        {
            soundUI.SoundUIPart.SetEventMuteButton(soundUI.AudioType);
        }
    }
}
