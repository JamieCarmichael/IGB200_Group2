using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: This script is destigned for logging data to a text file during testing.
/// </summary>
public class GatherTestData : MonoBehaviour 
{

    #region Fields
    /// <summary>
    /// The singleton instance of this object in the game.
    /// </summary>
    public static GatherTestData Instance { get; private set; }
    [Header("File")]
    [Tooltip("If this is true test data is logged. If false data is not logged.")]
    [SerializeField] private bool makeTestData = false;
    [Tooltip("The name of the folder within the games base directory. Include a / at the start eg. /TestData")]
    [SerializeField] private string folderName = "/TestData";
    [Tooltip("The files name. Include a / at the start eg. /test")]
    [SerializeField] private string fileName = "/test";
    [Tooltip("The file type. recommended .txt")]
    [SerializeField] private string fileType = ".txt";
    /// <summary>
    /// The folder that the game is being run in.
    /// </summary>
    private string exeFolder;
    /// <summary>
    /// The file path to for the file.
    /// </summary>
    private string filePath;
    [Header("Text fields")]
    [Tooltip("The text written when the doc is made. This is followed by the a data time.")]
    [SerializeField] private string startText = "Test Started: ";
    [Tooltip("The text written when this object is destroyed. This is followed by the a data time.")]
    [SerializeField] private string quitText = "Game Quit: ";

    private DateTime gameStartTime;

    private string GameTime
    {
        get
        {
            TimeSpan newTime = DateTime.Now - gameStartTime;
            return newTime.ToString(@"mm\:ss");
        }
    }
    #endregion


    #region Unity Call Functions
    private void Awake()
    {
        if (Application.isEditor)
        {
            makeTestData = false;
        }

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (makeTestData)
        {
            MakeFile();
        }
    }
    private void OnDestroy()
    {
        AddLineWithClockTime(quitText);
    }
    #endregion


    #region Write File Methods
    /// <summary>
    /// Make a file at the path. Adds start text to file.
    /// </summary>
    private void MakeFile()
    {
        // In case there is an issue in making the file.
        try
        {
            // Gets the directory the game is running in.
            exeFolder = Path.GetDirectoryName(Application.dataPath);
            // Make folder if it does not exist.
            if (!Directory.Exists(exeFolder + folderName))
            {
                Directory.CreateDirectory(exeFolder + folderName);
            }
            // Setting file path.
            string time = DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss");
            filePath = exeFolder + folderName + fileName + "_" + time + fileType;

            // Make file with initial text.
            AddLineWithClockTime(startText);
            //AddLineToFile("Time: " + Time.time);
        }
        catch (Exception)
        {
            Debug.Log("Could not make file" + filePath);
            Instance = null;
            Destroy(gameObject);
        }

    }
    /// <summary>
    /// Adds a line of text to the file with the time on the clock.
    /// </summary>
    /// <param name="nextLine">The next line of text to be added to the file.</param>
    public void AddLineWithClockTime(string nextLine)
    {
        if (makeTestData && filePath != null)
        {
            FileStream outFile = new FileStream(filePath, FileMode.Append);
            StreamWriter writer = new StreamWriter(outFile);
            writer.WriteLine(nextLine + DateTime.Now);
            writer.Close();
            outFile.Close();
        }
    }
    /// <summary>
    /// Adds a line of text to the file with the game time.
    /// </summary>
    /// <param name="nextLine">The next line of text to be added to the file.</param>
    public void AddLineWithGameTime(string nextLine)
    {
        if (makeTestData && filePath != null)
        {
            FileStream outFile = new FileStream(filePath, FileMode.Append);
            StreamWriter writer = new StreamWriter(outFile);
            writer.WriteLine(GameTime + ": " + nextLine);
            writer.Close();
            outFile.Close();
        }
    }
    /// <summary>
    /// Adds a line of text to the file.
    /// </summary>
    /// <param name="nextLine">The next line of text to be added to the file.</param>
    public void AddLineOfText(string nextLine)
    {
        if (makeTestData && filePath != null)
        {
            FileStream outFile = new FileStream(filePath, FileMode.Append);
            StreamWriter writer = new StreamWriter(outFile);
            writer.WriteLine(nextLine);
            writer.Close();
            outFile.Close();
        }
    }

    /// <summary>
    /// Resets the game timer.
    /// </summary>
    public void ResetGameTimer()
    {
        gameStartTime = DateTime.Now;
        AddLineWithClockTime("Clock reset at: ");
    }

    #endregion
}