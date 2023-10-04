using UnityEngine;
using TMPro;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A UI object that displays information for the player.
///             Should be part of the pause menu.
///             Contains tasks, inventory, profiles and a map.
/// </summary>
public class Notepad : MonoBehaviour 
{
    #region Fields
    public static Notepad Instance {  get; private set; }

    [Tooltip("The UI object that enables the notepad to be seen.")]
    [SerializeField] private GameObject notebookObject;

    /// <summary>
    /// If true the player has the notepad and it will be shown in the pause menu.
    /// </summary>
    [SerializeField] private bool notepadAquired = false;

    /// <summary>
    /// Pages on the notepad.
    /// </summary>
    public enum Pages
    {
        Tasks, Profile, Map
    }

    [Header("Buttons")]
    [Tooltip("The button to go to the next item in a page.")]
    [SerializeField] private GameObject nextButton;
    [Tooltip("The button to go to the previous item on a page.")]
    [SerializeField] private GameObject previousButton;

    [Header("Tasks")]
    [Tooltip("The TMP text field for the tasks.")]
    [SerializeField] private TextMeshProUGUI taskTextField;

    [Header("Profile")]
    [Tooltip("The profile manager for all of the profiles in the notepad.")]
    [SerializeField] private ProfileManager profileManager;

    [Header("Map")]
    [Tooltip("The panel that the Map UI is on.")]
    [SerializeField] private GameObject mapPanel;
    [Tooltip("The icon showing where the player is.")]
    [SerializeField] private RectTransform playerIconTransform;
    [Tooltip("The rect transform for the map image.")]
    [SerializeField] private RectTransform mapTransform;
    [Tooltip("The top right of the play area. Play area should be a square.")]
    [SerializeField] private Vector3 topRight;
    [Tooltip("The bottom left of the play area. Play area should be a square.")]
    [SerializeField] private Vector3 bottomLeft;

    /// <summary>
    /// The currrent page opened in the notepad.
    /// </summary>
    private Pages page = Pages.Tasks;

    private bool notepadVisable = false;

    [Header("UI prompt")]
    [Tooltip("The sprite shown when the notepad has been updated.")]
    [SerializeField] private Sprite notepadUpdateSprite;
    [Tooltip("How many seconds the prompt will be at full visability.")]
    [SerializeField] private float promptFullVisabilityTime = 1.0f;
    [Tooltip("How many second it will take for the prompt to got from full visability to not visable.")]
    [SerializeField] private float promtFadeTime = 1.0f;

    [Header("Audio")]
    [SerializeField] private Effect.Sound updateNotepadSound;
    private AudioSource audioSource;
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

        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        //ShowPage();

        InputManager.Instance.PlayerInput.PauseGame.Notepad.performed += context => ShowNotebook();
    }

    private void OnDisable()
    {
        InputManager.Instance.PlayerInput.PauseGame.Notepad.performed -= context => ShowNotebook();
    }
    #endregion

    #region Public Methods

    private void ShowPage()
    {
        // Select current page.
        switch (page)
        {
            case Pages.Tasks:
                ShowTasks();
                break;
            case Pages.Profile:
                ShowProfiles();
                break;
            case Pages.Map:
                ShowMap();
                break;
            default:
                break;
        }
    }
    private void ShowNotebook()
    {

        if (notepadAquired && !Pause.Instance.IsPaused)
        {
            notepadVisable = !notepadVisable;

            notebookObject.SetActive(notepadVisable);

            if (notepadVisable)
            {
                ShowPage();
                InputManager.Instance.PlayerInput.InGame.Disable();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                InputManager.Instance.PlayerInput.InGame.Enable();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

        }
    }
    /// <summary>
    /// Hide the notepad
    /// </summary>
    public void HideNotepad()
    {
        notepadVisable = false;
        notebookObject.SetActive(false);
    }

    /// <summary>
    /// Set if the notepad is visable in the Pause menu.
    /// </summary>
    /// <param name="isAquired">If true the notepad is able to be seen.</param>
    public void AquireNotepad(bool isAquired)
    {
        notepadAquired = isAquired;
    }


    /// <summary>
    /// Show the players tasks.
    /// </summary>
    private void ShowTasks()
    {
        page = Pages.Tasks;

        taskTextField.gameObject.SetActive(true);
        profileManager.HideProfile();;
        mapPanel.SetActive(false);

        // Set page buttons
        nextButton.SetActive(false);
        previousButton.SetActive(false);

        taskTextField.text = TaskManager.Instance.GetTaskListForNotepad();
    }

    /// <summary>
    /// Show the profile page.
    /// </summary>
    public void ShowProfiles()
    {
        page = Pages.Profile;

        taskTextField.gameObject.SetActive(false);
        mapPanel.SetActive(false);
        // Set page buttons
        nextButton.SetActive(profileManager.FindNextProfile() >= 0);
        previousButton.SetActive(profileManager.FindPreviousProfile() >= 0 );

        profileManager.DisplayProfile();
    }

    /// <summary>
    /// Show the map page.
    /// </summary>
    public void ShowMap()
    {
        page = Pages.Map;

        taskTextField.gameObject.SetActive(false);
        profileManager.HideProfile();
        mapPanel.SetActive(true);

        nextButton.SetActive(false);
        previousButton.SetActive(false);

        // Get the players position and place the player icon on that position on the map.
        Vector3 relitivePosition = PlayerManager.Instance.PlayerTransform.position - bottomLeft; // The players position relitive to the bottom left of the play area.
        Vector3 normalizedPos = relitivePosition / (topRight - bottomLeft).x; // Player position in play area between 0 and 1.
        Vector2 normalPosV2 = new Vector2(normalizedPos.x, normalizedPos.z); // ^ but Vector 2
        float mapSideLength = mapTransform.rect.width; // The length of a side of the map
        playerIconTransform.localPosition = (normalPosV2 * mapSideLength) - (new Vector2(mapSideLength, mapSideLength) / 2); // Move the icon position on the map.
    }

    /// <summary>
    /// Go to the next item on the page within the notepad.
    /// </summary>
    public void NextButton()
    {
        switch (page)
        {
            case Pages.Tasks:
                break;
            case Pages.Profile:
                {
                    if (profileManager.DisplayNextProfile())
                    {
                        nextButton.SetActive(true);
                    }
                    else
                    {
                        nextButton.SetActive(false);
                    }
                    previousButton.SetActive(true);
                    break;
                }
            case Pages.Map:
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// Go to the previous item on the page within the notepad.
    /// </summary>
    public void PreviousButton()
    {
        switch (page)
        {
            case Pages.Tasks:
                break;
            case Pages.Profile:
                {
                    if (profileManager.DisplayPreviousProfile())
                    {
                        previousButton.SetActive(true);
                    }
                    else
                    {
                        previousButton.SetActive(false);
                    }
                    nextButton.SetActive(true);
                    break;
                }
            case Pages.Map:
                break;
            default:
                break;
        }
    }

    public void UpdateNotepadPrompt()
    {
        UIManager.Instance.Prompt.StartPrompt(notepadUpdateSprite, promptFullVisabilityTime, promtFadeTime);
        audioSource.PlayOneShot(updateNotepadSound.clip, updateNotepadSound.volume);

    }
    #endregion
}