using UnityEngine;

public static class SaveManager
{
    private const string PlayerNameKey = "player_name";
    private const string MusicVolumeKey = "music_volume";
    private const string SfxVolumeKey = "sfx_volume";
    private const string HighScoreKey = "high_score";

    public static string GetPlayerName(string defaultname = "Player")
        => PlayerPrefs.GetString(PlayerNameKey, defaultname);
    public static void SetPlayerName(string name)
    {
        PlayerPrefs.SetString(PlayerNameKey, string.IsNullOrWhiteSpace(name) ? "Player" : name.Trim());
        PlayerPrefs.Save();
    }

    public static float GetMusicVolume(float defaultValue = 0.6f)
        => PlayerPrefs.GetFloat(MusicVolumeKey, Mathf.Clamp01(defaultValue));
    public static void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, Mathf.Clamp01(value));
        PlayerPrefs.Save();
    }

    public static float GetSfxVolume(float defaultValue = 0.8f) 
        => PlayerPrefs.GetFloat(SfxVolumeKey, Mathf.Clamp01(defaultValue));
    public static void SetSfxVolume(float value)
    {
        PlayerPrefs.SetFloat(SfxVolumeKey, Mathf.Clamp01(value));
        PlayerPrefs.Save();
    }

    public static int GetHighScore()
        => PlayerPrefs.GetInt(HighScoreKey, 0);
    public static void TrySetHighScore(int score)
    {
        int current = GetHighScore();
        if(score > current)
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
            PlayerPrefs.Save();
        }
    }
}
