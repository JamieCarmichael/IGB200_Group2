using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Uses sliders to save the sensitivity stat for the mouse. Saves values to player prefs to be used later.
/// </summary>
public class MouseSensitivityOption : MonoBehaviour 
{
    #region Fields
    private const string MOUSE_SENSITIVITY_X = "MouseSensitivityX";
    private const string MOUSE_SENSITIVITY_Y = "MouseSensitivityY";

    [Header("Mouse Sensitivity")]
    [Tooltip("The slider for the horizontal mouse sensitivity.")]
    [SerializeField] private Slider mouseSensitivityXSlider;
    [Tooltip("The slider for the vertical mouse sensitivity.")]
    [SerializeField] private Slider mouseSensitivityYSlider;

    private float defaultSensitivityValue = 10.0f;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        SetUpMouseSensitivity();
    }
    #endregion

    #region Public Methods

    /// <summary>
    /// Sets up the sliders for the mouse sensitivity.
    /// </summary>
    private void SetUpMouseSensitivity()
    {
        // Add listeners
        mouseSensitivityXSlider.onValueChanged.AddListener(SetMouseSensitivityX);
        mouseSensitivityYSlider.onValueChanged.AddListener(SetMouseSensitivityY);

        Vector2 mouseSensitivity = Vector2.one * defaultSensitivityValue;

        // Get saved value from player prefs
        mouseSensitivity.x = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_X, mouseSensitivity.x);
        mouseSensitivity.y = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_Y, mouseSensitivity.y);

        // Set default values
        mouseSensitivityXSlider.value = mouseSensitivity.x;
        mouseSensitivityYSlider.value = mouseSensitivity.y;
        SetMouseSensitivity();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Sets the mouse sensitivity on the horizontal axis. To be used by the slider.
    /// </summary>
    /// <param name="value"></param>
    private void SetMouseSensitivityX(float value)
    {
        PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_X, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Sets the mouse sensitivity on the verticle axis. To be used by the slider.
    /// </summary>
    /// <param name="value"></param>
    private void SetMouseSensitivityY(float value)
    {
        PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_Y, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Set the mouse sensitivity. Reads the values on the sliders.
    /// </summary>
    private void SetMouseSensitivity()
    {
        Vector2 sensitivity = new Vector2(mouseSensitivityXSlider.value, mouseSensitivityYSlider.value);

        PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_X, sensitivity.x);
        PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_Y, sensitivity.y);
        PlayerPrefs.Save();
    }
    #endregion

}