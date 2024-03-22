using UniRx;
using UnityEngine;

public class SoundUIPart : ValueUIPart
{
    [Header("ミュートボタン")]
    [SerializeField]
    private ToggleButton _muteButton;

    /// <summary>
    /// ミュートボタンのイベント設定
    /// </summary>
    /// <param name="type"> オーディオの種類 </param>
    public void SetEventMuteButton(AudioType type)
    {
        _muteButton.IsOn
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetMute(value, type);
            });
    }
}
