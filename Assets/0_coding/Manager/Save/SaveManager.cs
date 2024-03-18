using System;
using UnityEngine;

/// <summary>
/// セーブ管理
/// </summary>
public static class SaveManager
{
    private const int SOUND_INDEX = 4;
    private const string MOVE_SPEED_KEY = "moceSpeed";
    private const string SENSITIVITY_KEY = "Sensitivity";
    private const string IS_VERTICAL_REVERSE_KEY = "IsVerRev";
    private const string IS_HORIZONTAL_REVERSE_KEY = "IsHorRev";
    private const string MASTER_VOLUME_KEY = "Master";
    private const string BGM_VOLUME_KEY = "BGM";
    private const string SE_VOLUME_KEY = "SE";

    /// <summary>
    /// セーブデータからマウス操作が上下反転しているか取得
    /// </summary>
    /// <returns>反転しているか</returns>
    public static bool GetIsVerticalReverse()
    {
        int value = PlayerPrefs.GetInt(IS_VERTICAL_REVERSE_KEY, 0);
        return value > 0;
    }

    /// <summary>
    /// セーブデータからマウス操作が左右反転しているか取得
    /// </summary>
    /// <returns>反転しているか</returns>
    public static bool GetIsHorizontalReverse()
    {
        int value = PlayerPrefs.GetInt(IS_HORIZONTAL_REVERSE_KEY, 0);
        return value > 0;
    }

    /// <summary>
    /// セーブデータからプレイヤーのスピード取得
    /// </summary>
    /// <returns>感度</returns>
    public static float GetMoveSpeed()
    {
        return PlayerPrefs.GetFloat(MOVE_SPEED_KEY, 10f);
    }

    /// <summary>
    /// セーブデータからマウス感度取得
    /// </summary>
    /// <returns>感度</returns>
    public static float GetSensitivity()
    {
        return PlayerPrefs.GetFloat(SENSITIVITY_KEY, 10f);
    }

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
    /// セーブデータにマウス操作が上下反転しているか設定
    /// </summary>
    public static void SetSaveIsVerticalReverse(bool isReversed)
    {
        PlayerPrefs.SetInt(IS_VERTICAL_REVERSE_KEY, isReversed ? 1 : 0);
    }

    /// <summary>
    /// セーブデータにマウス操作が上下反転しているか設定
    /// </summary>
    public static void SetSaveIsHorizontalReverse(bool isReversed)
    {
        PlayerPrefs.SetInt(IS_HORIZONTAL_REVERSE_KEY, isReversed ? 1 : 0);
    }

    /// <summary>
    /// セーブデータにマウス感度設定
    /// </summary>
    public static void SetSaveMoveSpeed(float value)
    {
        PlayerPrefs.SetFloat(MOVE_SPEED_KEY, value);
    }

    /// <summary>
    /// セーブデータにマウス感度設定
    /// </summary>
    public static void SetSaveSensitivity(float value)
    {
        PlayerPrefs.SetFloat(SENSITIVITY_KEY, value);
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
