using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Chnages the game object being used for the central building.
/// </summary>
public class HouseProgress : MonoBehaviour 
{
    #region Fields
    public static HouseProgress Instance { get; private set; }

    [Serializable]
    public struct ProgressLevel
    {
        [Tooltip("The object being shown for this level of the building.")]
        public GameObject inGameObject;
        [Tooltip("The number of tasks required to build the next level of the building.")]
        public int tasksRequired;
    }
    [Tooltip("An array of the progress levels that this building can have.")]
    [SerializeField] private ProgressLevel[] progressLevels;

    /// <summary>
    /// Number of tasks done. Used for progressing to the next level.
    /// </summary>
    private int progressCount = 0;
    /// <summary>
    /// The index for the current progress level. 
    /// </summary>
    private int progressLevelIndex = 0;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        for (int i = 0; i < progressLevels.Length; i++)
        {
            progressLevels[i].inGameObject.SetActive(i == progressLevelIndex);
        }
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// Increase the number of tasks done to progress the building.
    /// </summary>
    public void IncreaseProgressLevel()
    {
        // If at final level stop.
        if (progressLevelIndex >= progressLevels.Length - 1)
        {
            return;
        }

        progressCount++;
        if (progressCount >= progressLevels[progressLevelIndex].tasksRequired)
        {
            progressLevels[progressLevelIndex].inGameObject.SetActive(false);
            progressLevelIndex++;
            progressLevels[progressLevelIndex].inGameObject.SetActive(true);
            progressCount = 0;
        }
    }
    #endregion
}