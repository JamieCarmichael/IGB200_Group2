using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Creates the player inputs.
/// </summary>
public class InputManager : MonoBehaviour 
{
    #region Fields
    public static InputManager Instance { get; private set; }
    public PlayerActions PlayerInput { get; private set; }
    #endregion

    #region Unity Call Functions
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        
        PlayerInput = new PlayerActions();

        PlayerInput.InGame.Enable();
        PlayerInput.PauseGame.Enable();
    }

    private void OnEnable()
    {
        PlayerInput.InGame.Enable();
    }
    private void OnDisable()
    {
        if (Instance == this)
        {
            PlayerInput.InGame.Disable();
        }
    }
    private void OnDestroy()
    {
        PlayerInput.Dispose();
    }
    #endregion
}