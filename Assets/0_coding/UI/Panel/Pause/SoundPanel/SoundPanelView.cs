using System.Collections.Generic;
using UnityEngine;

public class SoundPanelView : PanelViewBase
{
    [Header("音量UIリスト")]
    [SerializeField]
    private List<SoundUI> _soundUIList = new List<SoundUI>();
    /// <summary>
    /// 音量UIリスト
    /// </summary>
    public List<SoundUI> SoundUIList => _soundUIList;
    [Header("閉じるボタン")]
    [SerializeField]
    private ButtonBase _closeButton;
    /// <summary>
    /// 閉じるボタン
    /// </summary>
    public ButtonBase CloseButton => _closeButton;
}

[System.Serializable]
public class SoundUI
{
    [Header("音量の種類")]
    [SerializeField]
    private AudioType _audioType;
    /// <summary>
    /// 音量の種類
    /// </summary>
    public AudioType AudioType => _audioType;
    [Header("音量UI")]
    [SerializeField]
    private SoundUIPart _soundUIPart;
    /// <summary>
    /// 音量UI
    /// </summary>
    public SoundUIPart SoundUIPart => _soundUIPart;

}