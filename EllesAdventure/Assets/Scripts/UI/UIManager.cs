using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Manages scripts used in the UI. Holds references for scripts if they are needed.
/// </summary>
public class UIManager : MonoBehaviour 
{
    #region Fields
    public static UIManager Instance { get; private set; }

    private Notepad notepad;

    private UIPrompt prompt;

    /// <summary>
    /// The notepad object from the UI. Has Inventory and tasks.
    /// </summary>
    public Notepad Notepad { get { return notepad; } }

    /// <summary>
    /// The UI prompt used in this UI.
    /// </summary>
    public UIPrompt Prompt { get { return prompt; } }
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


        notepad = GetComponentInChildren<Notepad>();
        prompt = GetComponentInChildren<UIPrompt>();
    }
    #endregion
}