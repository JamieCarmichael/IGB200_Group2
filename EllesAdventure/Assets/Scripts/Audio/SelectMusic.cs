using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Sets the music being played by the music manager.
///             Placed in a scene so when it is started it will set the music for the scene.
/// </summary>
public class SelectMusic : MonoBehaviour 
{
    #region Fields
    [SerializeField] private MusicManager.MusicPlaying musicPlaying = MusicManager.MusicPlaying.MainMenu;
    
    [SerializeField] private VolumeSlider[] volumeSliders;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        MusicManager.Instance.SetMusic(musicPlaying);

        foreach (var volumeSlider in volumeSliders)
        {
            volumeSlider.Initialize();
        }
    }
    #endregion
}