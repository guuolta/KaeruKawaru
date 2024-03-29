using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
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
    private const string ENVIROMENTAL_VOLUME_NAME = "Environmental";
    private const string SE_VOLUME_NAME = "SE";
    private const string BGM_PITCH = "BGMPitch";
    private const string MAIN_PITCH = "MainPitch";

    private int[] _volumes = new int[SOUND_INDEX];
    private List<int> _volumeChangerList = new List<int>
    {
        0,15,28,40,51,63,68,76,82,88,90
    };
    [Header("オーディオミキサー")]
    [SerializeField]
    private AudioMixer _audioMixer;
    [Header("BGMのオーディオソース")]
    [SerializeField]
    private AudioSource _bgmAudioSource;
    [Header("環境音のオーディオソース")]
    [SerializeField]
    private AudioSource _enviromentalAudioSource;
    [Header("SEのオーディオソース")]
    [SerializeField]
    private AudioSource _seAudioSource;
    [Header("BGM")]
    [SerializeField]
    private List<BGMData> _bgmList = new List<BGMData>();
    [Header("よく使うSE")]
    [SerializeField]
    private List<SEData> _seList = new List<SEData>();
    [Header("カエルの鳴き声を鳴らす頻度")]
    [SerializeField]
    private float _frogSoundInterval = 10f;
    [Header("カエルの鳴き声が鳴る確率")]
    [Range(0, 100)]
    [SerializeField]
    private int _frogSoundProbability = 80;

    private List<AudioSource> _seAudioSourceList = new List<AudioSource>();
    private Dictionary<BGMType, AudioClip> _bgmDic = new Dictionary<BGMType, AudioClip>();
    private Dictionary<SEType, AudioClip> _seDict = new Dictionary<SEType, AudioClip>();

    protected override void Init()
    {
        base.Init();
        GetDictionary();
        SetInitVolume();
    }

    /// <summary>
    /// オーディオの辞書を取得
    /// </summary>
    private void GetDictionary()
    {
        foreach (var bgm in _bgmList)
            _bgmDic.Add(bgm.BGMType, bgm.Clip);

        foreach (var se in _seList)
            _seDict.Add(se.SEType, se.Clip);
    }

    /// <summary>
    /// 音量の初期値設定
    /// </summary>
    private void SetInitVolume()
    {
        _volumes = SaveManager.GetSoundVolumes();
        _audioMixer.SetFloat(MASTER_VOLUME_NAME, GetAudioMixerVolume(_volumes[(int)AudioType.Master]));
        _audioMixer.SetFloat(BGM_VOLUME_NAME, GetAudioMixerVolume(_volumes[(int)AudioType.BGM]));
        _audioMixer.SetFloat(SE_VOLUME_NAME, GetAudioMixerVolume(_volumes[(int)AudioType.SE]));
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventPlayFrog();
    }

    private void SetEventPlayFrog()
    {
        Observable
            .Interval(TimeSpan.FromSeconds(_frogSoundInterval))
            .TakeUntilDestroy(this)
            .Where(_ => GameStateManager.Status.Value == GameState.Play || GameStateManager.Status.Value == GameState.Title)
            .Select(_ => UnityEngine.Random.Range(0, 100))
            .Where(value => value <= _frogSoundProbability)
            .Subscribe(_ =>
            {
                PlayEnvironmental();
            });
    }


    /// <summary>
    /// オーディオミキサーに設定する音量
    /// </summary>
    /// <param name="volume"> 音量 </param>
    /// <returns></returns>
    private float GetAudioMixerVolume(int volume)
    {
        return -80 + _volumeChangerList[(int)volume];
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    public void PlayBGM(BGMType type)
    {
        if (type == BGMType.None)
            return;

        _bgmAudioSource.clip = _bgmDic[type];
        _bgmAudioSource.Play();
    }

    public void PlayEnvironmental()
    {
        _enviromentalAudioSource.Play();
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
                se.PlayOneShot(_seDict[type]);
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
        var seSource = Instantiate(_seAudioSource, transform).GetComponent<AudioSource>();
        _seAudioSourceList.Add(seSource);
    }

    /// <summary>
    /// SEを止める
    /// </summary>
    public void KillSE()
    {
        foreach (var se in _seAudioSourceList)
        {
            if(se.isPlaying)
            {
                se.Stop();
                se.clip = null;
            }
        }
    }

    /// <summary>
    /// ミュート設定
    /// </summary>
    /// <param name="isMute"> ミュートにするか </param>
    public void SetMute(bool isMute)
    {
        _bgmAudioSource.mute = isMute;
        _enviromentalAudioSource.mute = isMute;
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
                _enviromentalAudioSource.mute = isMute;
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
    public int[] GetSoundVolumes()
    {
        return _volumes;
    }

    /// <summary>
    /// 音量取得
    /// </summary>
    /// <returns></returns>
    public int GetSoundVolume(AudioType type)
    {
        return _volumes[(int)type];
    }

    /// <summary>
    /// 音量設定
    /// </summary>
    /// <param name="type"> オーディオの種類 </param>
    /// <param name="volume"> 音量 </param>
    public void SetVolume(AudioType type, int volume)
    {
        switch(type)
        {
            case AudioType.Master:
                _audioMixer.SetFloat(MASTER_VOLUME_NAME, GetAudioMixerVolume(volume));
                _volumes[(int)type] = volume;
                return;
            case AudioType.BGM:
                _audioMixer.SetFloat(BGM_VOLUME_NAME, GetAudioMixerVolume(volume));
                _audioMixer.SetFloat(ENVIROMENTAL_VOLUME_NAME, GetAudioMixerVolume(volume-1 > 0 ? volume-1 : 0));
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

    /// <summary>
    /// ピッチ変更
    /// </summary>
    public void ChangePitch(float _pitch)
    {
        _audioMixer.SetFloat(BGM_PITCH,_pitch);
        _audioMixer.SetFloat(MAIN_PITCH,1/_pitch);
    }
}

/// <summary>
/// BGM
/// </summary>
[System.Serializable]
public class BGMData
{
    [Header("BGMの種類")]
    [SerializeField]
    private BGMType _bgmType;
    /// <summary>
    /// BGMの種類
    /// </summary>
    public BGMType BGMType => _bgmType;
    [Header("BGMのクリップ")]
    [SerializeField]
    private AudioClip _clip;
    /// <summary>
    /// SEのクリップ
    /// </summary>
    public AudioClip Clip => _clip;
}

/// <summary>
/// SE
/// </summary>
[System.Serializable]
public class SEData
{
    [Header("SEの種類")]
    [SerializeField]
    private SEType _seType;
    /// <summary>
    /// SEの種類
    /// </summary>
    public SEType SEType => _seType;
    [Header("SEのクリップ")]
    [SerializeField]
    private AudioClip _clip;
    /// <summary>
    /// SEのクリップ
    /// </summary>
    public AudioClip Clip => _clip;
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
/// BGMの種類
/// </summary>
public enum BGMType
{
    None,
    Title,
    Main
}


/// <summary>
/// SEの種類
/// </summary>
public enum SEType
{
    None,
    Posi,
    Nega,
    Evo1,
    Evo2,
    Evo3,
    Fanfare
}