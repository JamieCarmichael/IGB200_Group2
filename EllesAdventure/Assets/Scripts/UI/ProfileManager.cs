using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Manages the NPC profiles within the notepad
/// </summary>
public class ProfileManager : MonoBehaviour 
{
    #region Fields
    [Tooltip("Array of all NPC profiles.")]
    [SerializeField] private Profile[] profiles;
    /// <summary>
    /// The current profile being viewed.
    /// </summary>
    private int currentProfile = 0;
    #endregion

    #region Public Methods
    /// <summary>
    /// Hide the profiles page.
    /// </summary>
    public void HideProfile()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Show the profiles page.
    /// </summary>
    public void DisplayProfile()
    {
        gameObject.SetActive(true);
        if (profiles[currentProfile].GetCurrentProfileState() != Profile.ProfileState.Unwritten)
        {
            profiles[currentProfile].DisplayProfile();
        }
        else
        {
            currentProfile = SelectNextProfile();

            if (currentProfile == -1)
            {
                currentProfile = 0;
            }
            else
            {
                profiles[currentProfile].DisplayProfile();
            }

        }
    }

    /// <summary>
    /// Display a profile for the NPC with the profileName.
    /// </summary>
    /// <param name="profileName"></param>
    public void DisplayProfile(string profileName)
    {
        for (int i = 0; i < profiles.Length; i++)
        {
            if (profiles[i].ProfileName == profileName)  
            {
                if (profiles[i].GetCurrentProfileState() != Profile.ProfileState.Unwritten)
                {
                    profiles[currentProfile].HideProfile();
                    currentProfile = i;
                    profiles[i].DisplayProfile();
                }
                break;
            }
        }
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Display the next profile. Will continue looking and loop around until it finds a profile.
    /// </summary>
    public void DisplayNextProfile()
    {
        profiles[currentProfile].HideProfile();
        currentProfile = SelectNextProfile();

        if (currentProfile == -1)
        {
            currentProfile = 0;
            return;
        }

        profiles[currentProfile].DisplayProfile();
    }
    /// <summary>
    /// Display the previous profile. Will continue looking and loop around until it finds a profile.
    /// </summary>
    public void DisplayPreviousProfile()
    {
        profiles[currentProfile].HideProfile();
        currentProfile = SelectPreviousProfile();

        if (currentProfile == -1)
        {
            currentProfile = 0;
            return;
        }

        profiles[currentProfile].DisplayProfile();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Find the next active profile.
    /// </summary>
    /// <returns></returns>
    private int SelectNextProfile()
    {
        int newProfileIndex = currentProfile;

        for (int i = 0; i < profiles.Length; i++)
        {
            newProfileIndex++;
            if (newProfileIndex >= profiles.Length)
            {
                newProfileIndex = 0;
            }
            if (profiles[newProfileIndex].GetCurrentProfileState() != Profile.ProfileState.Unwritten)
            {
                return newProfileIndex;
            }
        }

        return -1;
    }
    /// <summary>
    /// Find the previous active profile.
    /// </summary>
    /// <returns></returns>
    private int SelectPreviousProfile()
    {
        int newProfileIndex = currentProfile;

        for (int i = 0; i < profiles.Length; i++)
        {
            newProfileIndex--;
            if (newProfileIndex < 0)
            {
                newProfileIndex = profiles.Length - 1;
            }
            if (profiles[newProfileIndex].GetCurrentProfileState() != Profile.ProfileState.Unwritten)
            {
                return newProfileIndex;
            }
        }

        return -1;
    }
    #endregion

}