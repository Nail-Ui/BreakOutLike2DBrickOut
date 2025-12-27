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
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _restartButton;

    [SerializeField] private LevelSpawner _spawner;
    [SerializeField] private LevelManager _levelManager;

    private int _lives;
    private int _score;

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
        if (Input.GetKeyDown(KeyCode.Space) && _ball._isAttached)
        {
            _ball.LaunchBall();
        }
        if (_ball._isAttached)
        {
            _ball.BallIsAttached();
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

        if (_lives <= 0)
        {
            _lives = 0;
            GameOverPanel();
        }

        UpdateUI();
        AudioManager.Instance.PlaySfx(GameAudioLibrary.Instance._lifeLost);
        PaddleAndBallPosReset();
    }

    public void AddScore(int amount)
    {
        _score += amount;
        UpdateUI();
    }

    public void GameOverPanel()
    {
        _gameOverPanel.gameObject.SetActive(true);
        Time.timeScale = 0f;
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
        //SceneManager.LoadScene(1);
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
