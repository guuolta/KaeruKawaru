using UnityEngine;

public class SoundPanelView : PanelViewBase
{
    [Header("マスター音量UI")]
    [SerializeField]
    private ValueUIPart _masterValueUIPart;
    /// <summary>
    /// マスター音量UI
    /// </summary>
    public ValueUIPart MasterValueUIPart => _masterValueUIPart;
    [Header("BGM音量UI")]
    [SerializeField]
    private ValueUIPart _bgmValueUIParts;
    /// <summary>
    /// BGM音量UI
    /// </summary>
    public ValueUIPart BGMValueUIPart => _bgmValueUIParts;
    [Header("SE音量UI")]
    [SerializeField]
    private ValueUIPart _seValueUIParts;
    /// <summary>
    /// SE音量UI
    /// </summary>
    public ValueUIPart SEValueUIPart => _seValueUIParts;
    [Header("動画音量UI")]
    [SerializeField]
    private ValueUIPart _movieValueUIParts;
    /// <summary>
    /// 動画音量UI
    /// </summary>
    public ValueUIPart MovieValueUIPart => _movieValueUIParts;
}