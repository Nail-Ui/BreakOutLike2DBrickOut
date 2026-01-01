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
     public struct ScoreEntry
    {
        public string name;
        public int score;

        public ScoreEntry(string n, int s)
        {
            name = n;
            score = s;
        }
    }
    public static ScoreEntry[] GetTopScores()
    {
        ScoreEntry[] list = new ScoreEntry[3];

        for (int i = 0; i < 3; i++)
        {
            string name = PlayerPrefs.GetString($"hs_name_{i}", "---");
            int score = PlayerPrefs.GetInt($"hs_score_{i}", 0);
            list[i] = new ScoreEntry(name, score);
        }
        return list;
    }

    public static void TryAddScore(string playerName, int newScore)
    {
        ScoreEntry[] list = GetTopScores();

        //Listeye ekliyoruz
        ScoreEntry newEntry = new ScoreEntry(playerName, newScore);

        ScoreEntry[] combined = new ScoreEntry[4];
        for (int i = 0; i < 3; i++)
            combined[i] = list[i];

        combined[3] = newEntry;

        //Büyükten küçüğe sırala 
        System.Array.Sort(combined, (a, b) => b.score.CompareTo(a.score));

        //ilk 3'ü kaydediyoruz
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetString($"hs_name_{i}", combined[i].name);
            PlayerPrefs.SetInt($"hs_score_{i}", combined[i].score);
        }
        PlayerPrefs.Save();
    }

    public static int GetHighScore()
        => PlayerPrefs.GetInt(HighScoreKey, 0);
    public static void TrySetHighScore(int score)
    {
        int current = GetHighScore();
        if (score > current)
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
            PlayerPrefs.Save();
        }
    }

   
}
