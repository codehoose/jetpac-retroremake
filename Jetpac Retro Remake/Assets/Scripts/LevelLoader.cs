using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static string SCORE = nameof(SCORE);
    public static string HISCORE = nameof(HISCORE);
    public static string WAVE = nameof(WAVE);
    public static string LIVES = nameof(LIVES);
    public static int DefaultLives = 3;

    public static void LoadLevel(int wave, int score, int hiscore, int lives)
    {
        PlayerPrefs.SetInt(SCORE, score);
        PlayerPrefs.SetInt(HISCORE, hiscore);
        PlayerPrefs.SetInt(WAVE, wave);
        PlayerPrefs.SetInt(LIVES, lives);

        string sceneName = $"wave{wave % 8}"; // There are 8 enemies
        SceneManager.LoadScene(sceneName);
    }

    public static GameState GetGameState()
    {
        int score = PlayerPrefs.GetInt(SCORE, 0);
        int hiscore = PlayerPrefs.GetInt(HISCORE, 0);
        int wave = PlayerPrefs.GetInt(WAVE, 0);
        int lives = PlayerPrefs.GetInt(LIVES, 0);

        return new GameState { score = score, lives = lives, hiscore = hiscore, wave = wave };
    }
}

public struct GameState
{
    public int score;
    public int hiscore;
    public int wave;
    public int lives;
}
