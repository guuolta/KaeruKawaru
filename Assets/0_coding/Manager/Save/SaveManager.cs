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
    private const string FIRST_EASY_HIGH_SCORE_KEY = "EasyHighScore1";
    private const string SECOND_EASY_HIGH_SCORE_KEY = "EasyHighScore2";
    private const string THIRD_EASY_HIGH_SCORE_KEY = "EasyHighScore3";
    private const string FIRST_HARD_HIGH_SCORE_KEY = "HardHighScore1";
    private const string SECOND_HARD_HIGH_SCORE_KEY = "HardHighScore2";
    private const string THIRD_HARD_HIGH_SCORE_KEY = "HardHighScore3";

    /// <summary>
    /// 全音量取得
    /// </summary>
    /// <returns>音量</returns>
    public static float[] GetSoundVolumes()
    {
        float[] soundVolumes = new float[SOUND_INDEX];

        soundVolumes[(int)AudioType.Master] = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 8f);
        soundVolumes[(int)AudioType.BGM] = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 8f);
        soundVolumes[(int)AudioType.SE] = PlayerPrefs.GetFloat(SE_VOLUME_KEY, 8f);

        return soundVolumes;
    }

    /// <summary>
    /// イージーモードのハイスコアを取得
    /// </summary>
    /// <returns></returns>
    public static int[] GetEasyHighScores()
    {
        int[] highScores = new int[3];

        highScores[0] = PlayerPrefs.GetInt(FIRST_EASY_HIGH_SCORE_KEY, 0);
        highScores[1] = PlayerPrefs.GetInt(SECOND_EASY_HIGH_SCORE_KEY, 0);
        highScores[2] = PlayerPrefs.GetInt(THIRD_EASY_HIGH_SCORE_KEY, 0);

        return highScores;
    }

    /// <summary>
    /// ハードモードのハイスコアを取得
    /// </summary>
    /// <returns></returns>
    public static int[] GetHardHighScores()
    {
        int[] highScores = new int[3];

        highScores[0] = PlayerPrefs.GetInt(FIRST_HARD_HIGH_SCORE_KEY, 0);
        highScores[1] = PlayerPrefs.GetInt(SECOND_HARD_HIGH_SCORE_KEY, 0);
        highScores[2] = PlayerPrefs.GetInt(THIRD_HARD_HIGH_SCORE_KEY, 0);

        return highScores;
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
    /// イージーモードのハイスコア設定
    /// </summary>
    /// <param name="highScores"></param>
    public static void SetEasyHighScore(int[] highScores)
    {
        PlayerPrefs.SetInt(FIRST_EASY_HIGH_SCORE_KEY, highScores[0]);
        PlayerPrefs.SetInt(SECOND_EASY_HIGH_SCORE_KEY, highScores[1]);
        PlayerPrefs.SetInt(THIRD_EASY_HIGH_SCORE_KEY, highScores[2]);
    }

    /// <summary>
    /// ハードモードのハイスコア設定
    /// </summary>
    /// <param name="highScores"></param>
    public static void SetHardHighScore(int[] highScores)
    {
        PlayerPrefs.SetInt(FIRST_HARD_HIGH_SCORE_KEY, highScores[0]);
        PlayerPrefs.SetInt(SECOND_HARD_HIGH_SCORE_KEY, highScores[1]);
        PlayerPrefs.SetInt(THIRD_HARD_HIGH_SCORE_KEY, highScores[2]);
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
