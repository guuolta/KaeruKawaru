using System;
using UnityEngine;

/// <summary>
/// セーブ管理
/// </summary>
public static class SaveManager
{
    private const int SOUND_INDEX = 3;
    private const string MASTER_VOLUME_KEY = "Master";
    private const string BGM_VOLUME_KEY = "BGM";
    private const string SE_VOLUME_KEY = "SE";

    /// <summary>
    /// セーブデータから全音量取得
    /// </summary>
    /// <returns>音量</returns>
    public static float[] GetSoundVolume()
    {
        float[] _soundVolumes = new float[SOUND_INDEX];

        _soundVolumes[(int)AudioType.Master] = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 8f);
        _soundVolumes[(int)AudioType.BGM] = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 8f);
        _soundVolumes[(int)AudioType.SE] = PlayerPrefs.GetFloat(SE_VOLUME_KEY, 8f);

        return _soundVolumes;
    }

    /// <summary>
    /// セーブデータに音量をセット
    /// </summary>
    /// <param name="volumes"> 音量 </param>
    public static void SetSoundVolume(float[] volumes)
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volumes[(int)AudioType.Master]);
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volumes[(int)AudioType.BGM]);
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, volumes[(int)AudioType.SE]);
    }

    /// <summary>
    /// セーブ
    /// </summary>
    public static void Save()
    {
        PlayerPrefs.Save();
        Debug.Log("セーブ完了");
    }
}
