using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

public class SoundPanelPresenterBase : PanelPresenterBase<SoundPanelView>
{
    protected override void SetEvent()
    {
        SetValue();
        SetEventValueUIPart();
        SetEventMuteButton();
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        SetValue();
        await base.ShowAsync(ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        await base.HideAsync(ct);
        AudioManager.Instance.SaveVolume();
    }

    /// <summary>
    /// 初期値設定
    /// </summary>
    private void SetValue()
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
