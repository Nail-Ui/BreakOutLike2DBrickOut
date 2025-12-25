using UnityEngine;

public class GameAudioLibrary : MonoBehaviour
{
    public static GameAudioLibrary Instance { get; private set; }
    
    [Header("Music")]
    public AudioClip _menuMusic;
    public AudioClip _gameMusic;

    [Header("SFX")]
    public AudioClip _paddleHit;
    public AudioClip _wallHit;
    public AudioClip _lifeLost;
    public AudioClip _brickBreak;
    

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
}
