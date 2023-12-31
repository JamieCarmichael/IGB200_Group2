using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    int currentProfile = 0;

    [SerializeField] private TextMeshProUGUI nameTextField;
    [SerializeField] private TextMeshProUGUI bioTextField;
    [SerializeField] private TextMeshProUGUI detailsTextField;
    [SerializeField] private Image image;
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

        if (!profiles[currentProfile].GetProfileInfo(out Profile.ProfileInfo profileInfo))
        {
            nameTextField.enabled = false;
            bioTextField.enabled = false;
            detailsTextField.enabled = false;
            image.enabled = false;
            return;
        }

        nameTextField.text = profileInfo.name;
        bioTextField.text = profileInfo.bio;
        detailsTextField.text = profileInfo.details;
        image.sprite = profileInfo.image;

        nameTextField.enabled = true;
        bioTextField.enabled = true;
        detailsTextField.enabled = true;
        image.enabled = true;
    }

    /// <summary>
    /// Display the next profile. Will continue looking and loop around until it finds a profile.
    /// </summary>
    public bool DisplayNextProfile()
    {
        int nextProfile = FindNextProfile();
        if (nextProfile < 0)
        {
            return false;
        }
        currentProfile = nextProfile;
        DisplayProfile();

        return FindNextProfile() >= 0;
    }
    /// <summary>
    /// Display the previous profile. Will continue looking and loop around until it finds a profile.
    /// </summary>
    public bool DisplayPreviousProfile()
    {
        int nextProfile = FindPreviousProfile();
        if (nextProfile < 0)
        {
            return false;
        }
        currentProfile = nextProfile;
        DisplayProfile();

        return FindPreviousProfile() >= 0;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Find the next active profile.
    /// </summary>
    /// <returns></returns>
    public int FindNextProfile()
    {
        if (currentProfile < 0 || currentProfile >= profiles.Length)
        {
            return -1;
        }

        int newProfileIndex = currentProfile;

        for (int i = currentProfile; i < profiles.Length; i++)
        {
            newProfileIndex++;
            if (newProfileIndex >= profiles.Length)
            {
                return -1;
            }
            if (profiles[newProfileIndex].ProfileStageIndex >= 0)
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
    public int FindPreviousProfile()
    {
        int newProfileIndex = currentProfile;

        for (int i = currentProfile; i < profiles.Length; i--)
        {
            newProfileIndex--;
            if (newProfileIndex < 0)
            {
                return -1;
            }
            if (profiles[newProfileIndex].ProfileStageIndex >= 0)
            {
                return newProfileIndex;
            }
        }

        return -1;
    }

    public void EnableProfile(int profileIndex)
    {
        currentProfile = profileIndex;
    }
    #endregion

}