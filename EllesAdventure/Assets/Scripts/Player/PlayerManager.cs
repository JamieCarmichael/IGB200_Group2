using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Holds data for the player that needs to be accessable by other objects.
///             Players inventroy is held on this script.
/// </summary>
public class PlayerManager : MonoBehaviour 
{
    #region Fields
    public static PlayerManager Instance { get; private set; }

    [Tooltip("The players tarnsform. ")]
    [SerializeField] private Transform playerTransform;

    /// <summary>
    /// The players tarnsform.
    /// </summary>
    public Transform PlayerTransform
    {
        get { return playerTransform; }
    }

    private PlayerMovement playerMovement;
    private PlayerInteract playerInteract;
    private PlayerMovementLadder playerMovementLadder;
    private Inventory inventory;

    /// <summary>
    /// The players inventory.
    /// </summary>
    public Inventory playerInventory { get { return inventory; } }
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
        playerMovement = GetComponentInChildren<PlayerMovement>();
        playerInteract = GetComponentInChildren<PlayerInteract>();
        playerMovementLadder = GetComponentInChildren<PlayerMovementLadder>();
        inventory = GetComponent<Inventory>();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Sets the current player movement to use a ladder.
    /// </summary>
    /// <param name="ladder">The ladder to be used.</param>
    public void ClimbLadder(Ladder ladder)
    {
        playerMovementLadder.enabled = true;
        playerMovement.enabled = false;
        playerInteract.enabled = false;
        playerMovementLadder.AttachToLadder(ladder);
    }

    /// <summary>
    /// Sets up the current player movement to be normal movement.
    /// </summary>
    public void StandardMovement()
    {
        playerMovementLadder.enabled = false;
        playerMovement.enabled = true;
        playerInteract.enabled = true;
    }

    public void OnEdge()
    {
        playerMovementLadder.enabled = false;
        playerMovement.enabled = false;
        playerInteract.enabled = false;
    }
    #endregion
}