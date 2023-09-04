using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A NPC profile object within the notepad.
/// </summary>
public class Profile : MonoBehaviour
{
    #region Fields
    [Tooltip("The name of this NPC.")]
    [SerializeField] private string profileName;
    [Tooltip("The objects that are initalially visable when the profile is made.")]
    [SerializeField] private GameObject[] initialProfileObjects;
    [Tooltip("The objects that are visable once the profile is comleted.")]
    [SerializeField] private GameObject[] finalProfileObjects;

    private ProfileState currentProfileState = ProfileState.Unwritten;
    /// <summary>
    /// The posible profile states.
    /// </summary>
    public enum ProfileState
    {
        Unwritten,
        Initial,
        Final
    }
    /// <summary>
    /// Reutrns the name of this profile.
    /// </summary>
    public string ProfileName { get { return profileName; } }
    #endregion

    #region Public Methods
    /// <summary>
    /// Returns the current profile state.
    /// </summary>
    /// <returns></returns>
    public ProfileState GetCurrentProfileState()
    {
        return currentProfileState;
    }
    /// <summary>
    /// Make the initial state for profile visable in the notepad.
    /// </summary>
    public void MakeProfile()
    {
        currentProfileState = ProfileState.Initial;
        foreach (GameObject profileObject in finalProfileObjects)
        {
            profileObject.SetActive(false);
        }
        foreach (GameObject profileObject in initialProfileObjects)
        {
            profileObject.SetActive(true);
        }
    }
    /// <summary>
    /// Make the final state for profile visable in the notepad.
    /// </summary>
    public void FinishProfile()
    {
        currentProfileState = ProfileState.Final;
        foreach (GameObject profileObject in initialProfileObjects)
        {
            profileObject.SetActive(false);
        }
        foreach (GameObject profileObject in finalProfileObjects)
        {
            profileObject.SetActive(true);
        }
    }
    /// <summary>
    /// Make this profile visable
    /// </summary>
    public void DisplayProfile()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// Make this profile not visable.
    /// </summary>
    public void HideProfile()
    {
        gameObject.SetActive(false);
    }
    #endregion
}
