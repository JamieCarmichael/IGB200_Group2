using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Holds data for the player that needs to be accessable by other objects.
/// </summary>
public class PlayerManager : MonoBehaviour 
{
    #region Fields
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private Transform playerTransform;   

    public Transform PlayerTransform
    {
        get { return playerTransform; }
    }
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

    #region Public Methods

    #endregion

    #region Private Methods

    #endregion

}