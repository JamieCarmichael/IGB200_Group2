using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Manages scripts used in the UI. Holds references for scripts if they are needed.
/// </summary>
public class UIManager : MonoBehaviour 
{
    #region Fields
    public static UIManager Instance { get; private set; }

    [Tooltip("The notepad object in the UI.")]
    [SerializeField] private Notepad notepad;

    /// <summary>
    /// The notepad object from the UI. Has Inventory and tasks.
    /// </summary>
    public Notepad Notepad
    { get { return notepad; } }
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
    }
    #endregion
}