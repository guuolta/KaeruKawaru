using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class SoundPanelPresenter : PanelPresenterBase<SoundPanelView>
{
    protected override void SetEvent()
    {
        SetValue();
        SetEventValueUIPart();
        SetEventMuteButton();
        PausePanelManager.Instance.SetEventCloseButton(View.CloseButton, Ct);
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        SetValue();
        await base.ShowAsync(ct);
    }

    /// <summary>
    /// 初期値設定
    /// </summary>
    private void SetValue()
    {
        float[] volumes = AudioManager.Instance.GetSoundVolumes();

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
