using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

public class SoundPanelPresenter : PanelPresenterBase<SoundPanelView>
{
    protected override void SetEvent()
    {
        SetValue();
        SetEventValueUIPart();
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        SetValue();
        await base.ShowAsync(ct);
    }

    private void SetValue()
    {
        ////float[] volumes = AudioManager.Instance.GetSoundVolumes();

        ////View.MasterValueUIPart.SetValue(volumes[(int)AudioType.Master]);
        ////View.BGMValueUIPart.SetValue(volumes[(int)AudioType.BGM]);
        ////View.SEValueUIPart.SetValue(volumes[(int)AudioType.SE]);
        ////View.MovieValueUIPart.SetValue(volumes[(int)AudioType.Video]);
    }

    private void SetEventValueUIPart()
    {
        //View.MasterValueUIPart.Value
        //    .TakeUntilDestroy(this)
        //    .DistinctUntilChanged()
        //    .Subscribe(value =>
        //    {
        //        AudioManager.Instance.SetVolume(AudioType.Master, value);
        //    });

        //View.BGMValueUIPart.Value
        //    .TakeUntilDestroy(this)
        //    .DistinctUntilChanged()
        //    .Subscribe(value =>
        //    {
        //        AudioManager.Instance.SetVolume(AudioType.BGM, value);
        //    });

        //View.SEValueUIPart.Value
        //    .TakeUntilDestroy(this)
        //    .DistinctUntilChanged()
        //    .Subscribe(value =>
        //    {
        //        AudioManager.Instance.SetVolume(AudioType.SE, value);
        //    });

        //View.MovieValueUIPart.Value
        //    .TakeUntilDestroy(this)
        //    .DistinctUntilChanged()
        //    .Subscribe(value =>
        //    {
        //        AudioManager.Instance.SetVolume(AudioType.Video, value);
        //    });
    }
}
