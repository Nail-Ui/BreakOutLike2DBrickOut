using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField _playerNameInput;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private Button _startButton;

    [Header("Scenes")]
    [SerializeField] private int _gameSceneBuildIndex = 1;

    private void Start()
    {
        string name = SaveManager.GetPlayerName();
        float music = SaveManager.GetMusicVolume();
        float sfx = SaveManager.GetSfxVolume();
        int highScore = SaveManager.GetHighScore();

        if (_playerNameInput != null) _playerNameInput.SetTextWithoutNotify(name);
        if (_musicSlider != null) _musicSlider.SetValueWithoutNotify(music);
        if (_sfxSlider != null) _sfxSlider.SetValueWithoutNotify(sfx);
        if (_highScoreText != null) _highScoreText.text = $"High Score: {name}: {highScore}";

        // Sesleri hemen uygula
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ApplyVolumes(music, sfx);
        }
        //Eventleri ayarla
        if (_playerNameInput != null)
        {
            _playerNameInput.onEndEdit.RemoveListener(OnNameChanged);
            _playerNameInput.onEndEdit.AddListener(OnNameChanged);
        }

        if (_musicSlider != null)
        {
            _musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
            _musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }

        if (_sfxSlider != null)
        {
            _sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);
            _sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        }
        if (_startButton != null)
        {
            _startButton.onClick.RemoveListener(StartGame);
            _startButton.onClick.AddListener(StartGame);
        }
    }

    private void OnNameChanged(string value)
    {
        SaveManager.SetPlayerName(value);
    }

    private void OnMusicChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
        else
        {
            SaveManager.SetMusicVolume(value);
        }
    }
    private void OnSfxChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSfxVolume(value);
        }
        else
        {
            SaveManager.SetSfxVolume(value);
        }
    }
    private void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_gameSceneBuildIndex);
    }
}

