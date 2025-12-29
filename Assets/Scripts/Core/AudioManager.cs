using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    //Range(0f, 1f) Unity UI da slider olarak 0 ve 1 arasında float olan bir ayar/slider gösterir
    [Header("Defaults")]
    [SerializeField, Range(0f, 1f)] private float _defaultMusicVolume = 0.6f;
    [SerializeField, Range(0f, 1f)] private float _deafultSfxVolume = 0.8f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //Audio Source var mı yok mu bundan emin oluyoruz
        if (_musicSource == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
        }
        if (_sfxSource == null)
        {
            _sfxSource = gameObject.AddComponent<AudioSource>();
        }

        _musicSource.playOnAwake = false;
        _musicSource.loop = true;

        _sfxSource.playOnAwake = false;
        _sfxSource.loop = false;

        float musicVol = SaveManager.GetMusicVolume(_defaultMusicVolume);
        float sfxVol = SaveManager.GetSfxVolume(_deafultSfxVolume);
        ApplyVolumes(musicVol, sfxVol);
    }

    private void Start()
    {
        if (GameAudioLibrary.Instance != null)
        {
            PlayMusic(GameAudioLibrary.Instance._menuMusic, loop: true);
        }
    }

    public void ApplyVolumes(float musicVolume, float sfxVolume)
    {
        _musicSource.volume = Mathf.Clamp01(musicVolume);
        _sfxSource.volume = Mathf.Clamp01(sfxVolume);
    }

    public void SetMusicVolume(float value)
    {
        value = Mathf.Clamp01(value);
        _musicSource.volume = value;
        SaveManager.SetMusicVolume(value);
    }

    public void SetSfxVolume(float value)
    {
        value = Mathf.Clamp01(value);
        _sfxSource.volume = value;
        SaveManager.SetSfxVolume(value);
    }

    // Music helpers
    public void PlayMusic(AudioClip[] clips, bool loop = true)
    {
        if (clips == null || clips.Length == 0) return;
        
        AudioClip randomMusic = clips[Random.Range(0, clips.Length)];

        _musicSource.loop = loop;
        _musicSource.clip = randomMusic;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
        _musicSource.clip = null;
    }

    // SFX helpers
    public void PlaySfx(AudioClip[] clips, float volumeMultiplier = 1f)
    {
        if (clips == null || clips.Length == 0) return; //Boş array kontrolü

        // Random clip seç (Her seferinde farklı bir clip seçilir)
        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        _sfxSource.PlayOneShot(randomClip, Mathf.Clamp01(volumeMultiplier));
    }

    public void StopSfx()
    {
        _sfxSource.Stop();
    }


}
