using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Allows the game to pause and unpause.
///             Has methods for buttons in the pause menu.
/// </summary>
public class Pause : MonoBehaviour 
{
    #region Fields
    [Tooltip("The UI panel that is the visual for the pause menu.")]
    [SerializeField] private GameObject pausePanel;
    [Tooltip("Objects to be disabled when the game is paused.")]
    [SerializeField] private GameObject[] UIObjectsToHide;

    /// <summary>
    /// If ture the game is paused.
    /// </summary>
    private bool isPaused = false;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        PauseGame(false);
    }
    private void OnEnable()
    {
        InputManager.Instance.PlayerInput.PauseGame.Pause.performed += context => PauseGame();
    }
    private void OnDisable()
    {
        InputManager.Instance.PlayerInput.PauseGame.Pause.performed -= context => PauseGame();
    }
    #endregion

    #region Pause Methods
    /// <summary>
    /// Will toggle from pause to unpausing the game.
    /// Shows UI panel, stops time, stops inputs, sets cursor visability.
    /// </summary>
    public void PauseGame()
    {
        PauseGame(!isPaused);
    }

    /// <summary>
    /// Sets the game to pause or unpause. True pauses the game.
    /// </summary>
    /// <param name="stopGame">Pause the game if true.</param>
    private void PauseGame(bool stopGame)
    {
        if (stopGame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isPaused = true;
            Time.timeScale = 0.0f;
            pausePanel.SetActive(true);
            InputManager.Instance.PlayerInput.InGame.Disable();

            for (int i = 0; i < UIObjectsToHide.Length; i++)
            {
                UIObjectsToHide[i].SetActive(false);
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isPaused = false;
            Time.timeScale = 1.0f;
            pausePanel.SetActive(false);
            InputManager.Instance.PlayerInput.InGame.Enable();

            for (int i = 0; i < UIObjectsToHide.Length; i++)
            {
                UIObjectsToHide[i].SetActive(true);
            }
        }
    }
    #endregion

    #region Pause Buttons
    /// <summary>
    /// Switch to the new scene.
    /// </summary>
    /// <param name="newScene"></param>
    public void SwitchScene(string newScene)
    {
        SceneNavigation.Instance.SwitchScene(newScene);
    }

    /// <summary>
    /// Reload the current scene.
    /// </summary>
    public void ReloadScene()
    {
        SceneNavigation.Instance.ReloadScene();
    }
    #endregion
}