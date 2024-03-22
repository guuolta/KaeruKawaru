using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// オーディオを管理
/// </summary>
public class AudioManager : DontDestroySingletonObject<AudioManager>
{
    private const int SOUND_INDEX = 3;
    private const string MASTER_VOLUME_NAME = "Master";
    private const string BGM_VOLUME_NAME = "BGM";
    private const string SE_VOLUME_NAME = "SE";

    private float[] _volumes = new float[SOUND_INDEX];
    [Header("オーディオミキサー")]
    [SerializeField]
    private AudioMixer _audioMixer;
    [Header("BGMのオーディオソース")]
    [SerializeField]
    private AudioSource _bgmAudioSource;
    [Header("SEのオーディオソース")]
    [SerializeField]
    private AudioSource _seAudioSource;
    [Header("通常BGM")]
    [SerializeField]
    private AudioClip _mainAudioClip;
    [Header("よく使うSE")]
    [SerializeField]
    private List<SE> _seList = new List<SE>();

    private List<AudioSource> _seAudioSourceList = new List<AudioSource>();
    private Dictionary<SEType, AudioClip> _seDictionary = new Dictionary<SEType, AudioClip>();

    protected override void Init()
    {
        base.Init();
        GetSEDictionary();
        SetInitVolume();
        PlayMainBGM();
    }

    /// <summary>
    /// SEの辞書を取得
    /// </summary>
    private void GetSEDictionary()
    {
        foreach (var se in _seList)
            _seDictionary.Add(se.SEType, se.Clip);
    }

    /// <summary>
    /// 音量の初期値設定
    /// </summary>
    private void SetInitVolume()
    {
        _volumes = SaveManager.GetSoundVolume();
        _audioMixer.SetFloat(MASTER_VOLUME_NAME, GetAudioMixerVolume(_volumes[(int)AudioType.Master]));
        _audioMixer.SetFloat(BGM_VOLUME_NAME, GetAudioMixerVolume(_volumes[(int)AudioType.BGM]));
        _audioMixer.SetFloat(SE_VOLUME_NAME, GetAudioMixerVolume(_volumes[(int)AudioType.SE]));
    }

    /// <summary>
    /// オーディオミキサーに設定する音量
    /// </summary>
    /// <param name="volume"> 音量 </param>
    /// <returns></returns>
    private float GetAudioMixerVolume(float volume)
    {
        return -80 + volume * 10;
    }

    /// <summary>
    /// メインBGM再生
    /// </summary>
    public void PlayMainBGM()
    {
        if (_bgmAudioSource.clip == null)
            _bgmAudioSource.clip = _mainAudioClip;
        if (_bgmAudioSource.isPlaying)
            return;

        _bgmAudioSource.Play();
    }

    /// <summary>
    /// メインBGM停止
    /// </summary>
    public void PauseMainBGM()
    {
        if (!_bgmAudioSource.isPlaying)
            return;

        _bgmAudioSource.Pause();
    }

    /// <summary>
    /// SEを鳴らす
    /// </summary>
    /// <param name="clip"> 鳴らすSE </param>
    public void PlayOneShotSE(AudioClip clip)
    {
        foreach (AudioSource se in _seAudioSourceList)
        {
            if(!se.isPlaying)
            {
                se.PlayOneShot(clip);
                return;
            }
        }

        CreateSEAudioSource();
        PlayOneShotSE(clip);
    }

    /// <summary>
    /// SEを鳴らす
    /// </summary>
    /// <param name="type"> Seの種類 </param>
    public void PlayOneShotSE(SEType type)
    {
        if(type == SEType.None)
            return;

        foreach (AudioSource se in _seAudioSourceList)
        {
            if (!se.isPlaying)
            {
                se.PlayOneShot(_seDictionary[type]);
                return;
            }
        }

        CreateSEAudioSource();
        PlayOneShotSE(type);
    }

    /// <summary>
    /// SEのオーディオソースを生成
    /// </summary>
    private void CreateSEAudioSource()
    {
        var seSource = Instantiate(_seAudioSource).GetComponent<AudioSource>();
        _seAudioSourceList.Add(seSource);
    }

    /// <summary>
    /// ミュート設定
    /// </summary>
    /// <param name="isMute"> ミュートにするか </param>
    public void SetMute(bool isMute)
    {
        _bgmAudioSource.mute = isMute;
        _seAudioSource.mute = isMute;
    }

    /// <summary>
    /// ミュート設定
    /// </summary>
    /// <param name="isMute"> ミュートにするか </param>
    public void SetMute(bool isMute, AudioType type)
    {
        switch (type)
        {
            case AudioType.Master:
                SetMute(isMute);
                break;
            case AudioType.BGM:
                _bgmAudioSource.mute = isMute;
                break;
            case AudioType.SE:
                _seAudioSource.mute = isMute;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 音量取得
    /// </summary>
    /// <returns></returns>
    public float[] GetSoundVolumes()
    {
        return _volumes;
    }

    /// <summary>
    /// 音量取得
    /// </summary>
    /// <returns></returns>
    public float GetSoundVolume(AudioType type)
    {
        return _volumes[(int)type];
    }

    /// <summary>
    /// 音量設定
    /// </summary>
    /// <param name="type"> オーディオの種類 </param>
    /// <param name="volume"> 音量 </param>
    public void SetVolume(AudioType type, float volume)
    {
        switch(type)
        {
            case AudioType.Master:
                _audioMixer.SetFloat(MASTER_VOLUME_NAME, GetAudioMixerVolume(volume));
                _volumes[(int)type] = volume;
                return;
            case AudioType.BGM:
                _audioMixer.SetFloat(BGM_VOLUME_NAME, GetAudioMixerVolume(volume));
                break;
            case AudioType.SE:
                _audioMixer.SetFloat(SE_VOLUME_NAME, GetAudioMixerVolume(volume));
                break;
            default:
                return;
        }

        _volumes[(int)type] = volume;
    }

    /// <summary>
    /// すべてのボリュームをセーブ
    /// </summary>
    public void SaveVolume()
    {
        SaveManager.SetSoundVolume(_volumes);
    }
}

[System.Serializable]
public class SE
{
    public SEType SEType;
    public AudioClip Clip;
}

/// <summary>
/// オーディオの種類
/// </summary>
public enum AudioType
{
    Master = 0,
    BGM = 1,
    SE = 2
}

/// <summary>
/// SEの種類
/// </summary>
public enum SEType
{
    None,
    Posi,
    Nega
}