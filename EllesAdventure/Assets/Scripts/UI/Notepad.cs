using System.Collections.Generic;
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

    /// <summary>
    /// A list of all currenly active tasks.
    /// </summary>
    private List<OldSODoTask> activeTasks = new List<OldSODoTask>();

    [Tooltip("The UI object that enables the notepad to be seen.")]
    [SerializeField] private GameObject notebookObject;

    /// <summary>
    /// If true the player has the notepad and it will be shown in the pause menu.
    /// </summary>
    private bool notepadAquired = false;

    /// <summary>
    /// Pages on the notepad.
    /// </summary>
    public enum Pages
    {
        Tasks, Inventroy, Profile, Map
    }

    [Header("Buttons")]
    [Tooltip("The button to go to the next item in a page.")]
    [SerializeField] private GameObject nextButton;
    [Tooltip("The button to go to the previous item on a page.")]
    [SerializeField] private GameObject previousButton;

    [Header("Tasks")]
    [Tooltip("The panel that the tasks UI is on.")]
    [SerializeField] private GameObject tasksPanel;
    [Tooltip("The TMP text field for the tasks name.")]
    [SerializeField] private TextMeshProUGUI taskName;
    [Tooltip("The TMP text field for the tasks description.")]
    [SerializeField] private TextMeshProUGUI taskDescription;
    [Tooltip("The text displayed if there are no active tasks.")]
    [SerializeField] private string noTaskText = "No Task";
    /// <summary>
    /// The current task being viewed.
    /// </summary>
    private int currentTask = 0;

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

    private void OnEnable()
    {
        ShowPage();

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
        gameObject.SetActive(notepadAquired);
    }
    /// <summary>
    /// Add a task to the player tasks.
    /// </summary>
    /// <param name="newTask"></param>
    public void AddTask(OldSODoTask newTask)
    {
        UpdateNotepadPrompt();
        activeTasks.Add(newTask);
    }
    /// <summary>
    /// Remove a task from the players tasks.
    /// </summary>
    /// <param name="task"></param>
    public void RemoveTask(OldSODoTask task)
    {
        activeTasks.Remove(task);
    }

    /// <summary>
    /// Show the players tasks.
    /// </summary>
    public void ShowTasks()
    {
        if (currentTask >= activeTasks.Count)
        {
            currentTask = activeTasks.Count - 1;
        }
        else if (currentTask < 0)
        {
            currentTask = 0;
        }
        ShowTasks(currentTask);
    }

    /// <summary>
    /// Show the players tasks.
    /// </summary>
    /// <param name="taskNumber"></param>
    private void ShowTasks(int taskNumber)
    {
        if (taskNumber >= activeTasks.Count)
        {
            taskNumber = activeTasks.Count - 1;
        }
        else if (taskNumber < 0)
        {
            taskNumber = 0;
        }
        page = Pages.Tasks;

        tasksPanel.SetActive(true);
        profileManager.HideProfile();;
        mapPanel.SetActive(false);

        // Set page buttons
        if (taskNumber >= activeTasks.Count - 1)
        {
            nextButton.SetActive(false);
        }
        else
        {
            nextButton.SetActive(true);
        }
        if (taskNumber <= 0)
        {
            previousButton.SetActive(false);
        }
        else
        {
            previousButton.SetActive(true);
        }

        // Set active task
        if (activeTasks.Count == 0)
        {
            taskName.text = noTaskText;
            taskDescription.text = "";
        }
        else
        {
            taskName.text = activeTasks[taskNumber].TaskName;
            taskDescription.text = activeTasks[taskNumber].Description;
        }
    }

    /// <summary>
    /// Show the profile page.
    /// </summary>
    public void ShowProfiles()
    {
        page = Pages.Profile;

        tasksPanel.SetActive(false);
        mapPanel.SetActive(false);
        // Set page buttons
        nextButton.SetActive(true);
        previousButton.SetActive(true);

        profileManager.DisplayProfile();
    }

    /// <summary>
    /// Show the map page.
    /// </summary>
    public void ShowMap()
    {
        page = Pages.Map;

        tasksPanel.SetActive(false);
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
                currentTask += 1;
                ShowTasks(currentTask);
                break;
            case Pages.Profile:
                profileManager.DisplayNextProfile();
                break;
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
                currentTask -= 1;
                ShowTasks(currentTask);
                break;
            case Pages.Profile:
                profileManager.DisplayPreviousProfile();
                break;
            case Pages.Map:
                break;
            default:
                break;
        }
    }

    public void UpdateNotepadPrompt()
    {
        UIManager.Instance.Prompt.StartPrompt(notepadUpdateSprite, promptFullVisabilityTime, promtFadeTime);
    }
    #endregion
}