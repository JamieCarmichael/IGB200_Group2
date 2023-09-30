using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A simple music manager.
/// </summary>
public class MusicManager : MonoBehaviour 
{
    #region Fields
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioClip[] mainMenuMusic;
    [SerializeField] private AudioClip[] inGameMusic;

    [Tooltip("If true the next clip is randomly selected.")]
    [SerializeField] private bool randomClip;

    [SerializeField] private bool finishSongBeforeChangeMusic = true;

    private MusicPlaying currentMusicPlaying = MusicPlaying.MainMenu;

    private int currentClipIndex = 0;

    private AudioSource audioSource;

    public enum MusicPlaying
    {
        MainMenu,
        InGame
    }
    #endregion

    #region Unity Call Functions
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextClip();
        }
    }
    #endregion

    #region Public Methods
    public void SetMusic(MusicPlaying newMusicPlaying)
    {
        currentMusicPlaying = newMusicPlaying;
        currentClipIndex = -1;
        if (!finishSongBeforeChangeMusic)
        {
            PlayNextClip();
        }
    }
    #endregion

    #region Private Methods
    private void PlayNextClip()
    {
        if (randomClip)
        {
            SelectNextTrackRandom();
        }
        else
        {
            SelectNextTrackLinear();
        }
    }
    private void SelectNextTrackRandom()
    {
        AudioClip[] currentClips = GetCurrentClips();
        if (!HasAudioClips(currentClips))
        {
            return;
        }

        // Select random clip
        int newClipIndex = Random.Range(0, currentClips.Length);
        if (newClipIndex == currentClipIndex)
        {
            newClipIndex++;
            if (newClipIndex >= currentClips.Length)
            {
                newClipIndex = 0;
            }
        }
        currentClipIndex = newClipIndex;

        audioSource.clip = currentClips[currentClipIndex];
        audioSource.Play();
    }
    private void SelectNextTrackLinear()
    {
        AudioClip[] currentClips = GetCurrentClips();
        if (!HasAudioClips(currentClips))
        {
            return;
        }

        currentClipIndex++;
        if (currentClipIndex >= currentClips.Length)
        {
            currentClipIndex = 0;
        }
        audioSource.clip = currentClips[currentClipIndex];
        audioSource.Play();
    }
    private AudioClip[] GetCurrentClips()
    {
        switch (currentMusicPlaying)
        {
            case MusicPlaying.MainMenu:
                {
                    return mainMenuMusic;
                }
            case MusicPlaying.InGame:
                {
                    return inGameMusic;
                }
            default:
                return mainMenuMusic;
        }
    }

    private bool HasAudioClips(AudioClip[] currentClips)
    {
        if (currentClips.Length == 0)
        {
            Debug.LogWarning(currentMusicPlaying.ToString() + " has no audio clips. Disabling Music Manager!");
            gameObject.SetActive(false);
            return false;
        }
        return true;
    }
    #endregion
}