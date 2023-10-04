using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: This script controls a volume paramater within a audio mixer with a slider.
/// </summary>
public class VolumeSlider : MonoBehaviour 
{
    #region Fields
    [Range(0,1)]
    [SerializeField] float defaultValue = 0.7f;

    [Tooltip("This is the audio mixer being used in to set the volume.")]
    [SerializeField] AudioMixer audioMixer;
    [Tooltip("This is the volume parameter that is being changed.")]
    [SerializeField] string volumeParameter;
    [Tooltip("This is the slider controlling the colume")]
    [SerializeField] Slider slider;
    [Tooltip("This is the multiplier being used when setting the volume. It will chage how quite the volume can be set to. Recomended 30.")]
    [SerializeField] float volumeMultiplier = 30.0f;
    [Tooltip("This is the volume when the slider is at 0. Recomented 0.01.")]
    [SerializeField] float minVolume = 0.01f;
    #endregion

    #region Unity Call Functions
    private void OnEnable()
    {
        Initialize();
    }
    #endregion

    #region Public Method
    public void Initialize()
    {
        slider.onValueChanged.AddListener(HandleSliderValue);
        //Loads the value so that it is the same as the last time the game was played.
        float volumeValue = PlayerPrefs.GetFloat(volumeParameter, defaultValue);
        slider.value = volumeValue;
        HandleSliderValue(volumeValue);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// This method sets the volume in the mixer to the value.
    /// </summary>
    /// <param name="volumeValue"></param>
    private void HandleSliderValue(float volumeValue)
    {
        if (volumeValue < 0.05)
        {
            audioMixer.SetFloat(volumeParameter, value: Mathf.Log10(minVolume) * volumeMultiplier);
            //Saves the value so that it is kept when quiting and restarting the game.
            PlayerPrefs.SetFloat(volumeParameter, minVolume);
        }
        else
        {
            audioMixer.SetFloat(volumeParameter, value: Mathf.Log10(volumeValue) * volumeMultiplier);
            //Saves the value so that it is kept when quiting and restarting the game.
            PlayerPrefs.SetFloat(volumeParameter, volumeValue);
        }

    }
    #endregion

}