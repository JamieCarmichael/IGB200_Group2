using System;
using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A NPC profile object within the notepad.
/// </summary>
public class Profile : MonoBehaviour
{
    [Serializable]
    public struct ProfileInfo
    {
        public string name;
        public string bio;
        public string details;
        public Sprite image;
    }

    #region Fields
    [SerializeField] private ProfileManager profileManager;

    [SerializeField] private int profileIndex = -1;

    [SerializeField] private ProfileInfo[] profileStages;

    public int profileStageIndex = -1;

    public int ProfileStageIndex 
    {  
        get 
        { 
            return profileStageIndex; 
        }
        set
        {
            if (value >= profileStages.Length)
            {
                profileStageIndex = profileStages.Length - 1;
            }
            else if (value < 0)
            {
                profileStageIndex = 0;
            }
            profileStageIndex = value;
        }
    }
    public ProfileInfo CurrentProfile
    {
        get
        {
            if (profileStageIndex < 0)
            {
                return profileStages[0];
            }
            if (profileStageIndex >= profileStages.Length)
            {
                return profileStages[profileStages.Length - 1];
            }
            return profileStages[profileStageIndex];
        }
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// Make the initial state for profile visable in the notepad.
    /// </summary>
    public bool GetProfileInfo(out ProfileInfo profileInfo)
    {
        if (profileStageIndex < 0)
        {
            profileInfo = new ProfileInfo();
            return false;
        }
        profileInfo = profileStages[profileStageIndex];
        return true;
    }

    public void IncreaseProfileStage()
    {
        ProfileStageIndex++;
        profileManager.EnableProfile(profileIndex);
    }
    #endregion
}
