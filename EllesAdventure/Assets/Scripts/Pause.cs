using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>
public class Pause : MonoBehaviour 
{
    #region Fields
    [SerializeField] private GameObject pausePanel;

    private bool isPaused = false;
    #endregion

    #region Unity Call Functions

    #endregion

    #region Public Methods
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.Instance.PlayerInput.PauseGame.Pause.performed += context => PauseGame();
    }
    #endregion

    #region Private Methods
    private void PauseGame()
    {
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isPaused = false;
            Time.timeScale = 1.0f;
            pausePanel.SetActive(false);
            //InputManager.Instance.PlayerInput.InGame.Disable();

        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isPaused = true;
            Time.timeScale = 0.0f;
            pausePanel.SetActive(true);
            //InputManager.Instance.PlayerInput.InGame.Enable();
        }
    }
    #endregion

}