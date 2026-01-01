using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Gameplay")]
    [SerializeField] private Ball _ball;
    [SerializeField] private Paddle _paddle;
    [SerializeField] private int _startingLives = 3;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _livesText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _restartButton;

    [SerializeField] private LevelSpawner _spawner;
    [SerializeField] private LevelManager _levelManager;

    private int _lives;
    private int _score;
    private bool _isPaused = false;
    private bool _gameEnded = false;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        ResetLivesAndScore();
        SpawnCurrentLevel();
        _gameOverPanel.SetActive(false);

        if (GameAudioLibrary.Instance != null)
        {
            AudioManager.Instance.PlayMusic(GameAudioLibrary.Instance._gameMusic, loop: true);
        }
    }

    private void Update()
    {
        HandleLaunchInput();

        if (_ball._isAttached)
        {
            _ball.FollowPaddle();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void HandleLaunchInput()
    {
        if (!_ball._isAttached || _isPaused || _gameEnded) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _ball.LaunchBall();
        }
    }
    private void SpawnCurrentLevel()
    {
        _spawner.Spawn(_levelManager.Current);
        PaddleAndBallPosReset();
    }

    public void OnBallLoss()
    {
        _lives--;

        AudioManager.Instance.PlaySfx(GameAudioLibrary.Instance._lifeLostClips);
        
        if (_lives <= 0)
        {
            _lives = 0;
            UpdateUI();
            GameOverPanel();
            return;
        }

        UpdateUI();
        PaddleAndBallPosReset();
    }

    public void AddScore(int amount)
    {
        _score += amount;
        UpdateUI();
    }

    public void GameOverPanel()
    {
        if(_gameEnded) return;
        _gameEnded = true;

        _gameOverPanel.gameObject.SetActive(true);

        //SaveManager.TrySetHighScore(_score);

        string playerName = SaveManager.GetPlayerName();
        SaveManager.TryAddScore(playerName, _score);

        Time.timeScale = 0f;
        
    }
    public void PauseGame()
    {
        if (_isPaused) return;

        _pausePanel.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }
    public void ResumeGame()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void ReturnToMainMenu()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.StopSfx();
        SceneManager.LoadScene(0);
        AudioManager.Instance.PlayMusic(GameAudioLibrary.Instance._menuMusic, loop: true);
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    public void RestartTheLevel()
    {
        AudioManager.Instance.StopSfx();
        _levelManager.ResetToFirstLevel();
        ResetLivesAndScore();
        PaddleAndBallPosReset();
        SpawnCurrentLevel();
        _gameOverPanel.SetActive(false);
        _pausePanel.SetActive(false);
        //SceneManager.LoadScene(1);
        _gameEnded = false;
        Time.timeScale = 1f;
    }

    private void UpdateUI()
    {
        if (_livesText != null) _livesText.text = $"Lives: {_lives}";
        if (_scoreText != null) _scoreText.text = $"Score: {_score}";
    }

    public void PaddleAndBallPosReset()
    {
        _ball.ResetBallPosition();
        _paddle.ResetPaddlePosition();
    }

    private void ResetLivesAndScore()
    {
        _lives = _startingLives;
        _score = 0;
        UpdateUI();
    }

    public void OnLevelCleared()
    {

        PaddleAndBallPosReset();

        bool hasNext = _levelManager.TryGoNextLevel();
        if (hasNext)
        {
            Invoke(nameof(SpawnCurrentLevel), 1f);
        }
        else
        {
            // Tüm level'lar bitti: win ekranı yoksa main menu eklenebilir
            // Buraya "You Win" paneli + highscore eklenebilir
            Invoke(nameof(BackToMenuOrWin), 1f);
        }
    }

    private void BackToMenuOrWin()
    {
        //
    }

    // private void ResetAndSpawnNextLevel()
    // {
    //     _spawner.Spawn(LevelManage);
    // }
}
