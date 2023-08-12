using UnityEngine;
using System.Collections;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Enables the player to interact with objects with the IInteractable interface.
/// </summary>
public class PlayerInteract : MonoBehaviour 
{
    #region Fields
    [Header("Interact")]
    [Tooltip("The maximum interacting distance from an object.")]
    [SerializeField] private float interactRange = 1.0f;
    [Tooltip("The distance from an intarctable object that the player will move to.")]
    [SerializeField] private float stoppingDistance = 0.5f;
    [Tooltip("The layer for interactable objects.")]
    [SerializeField] private LayerMask interactLayer;
    /// <summary>
    /// The current interactable object.
    /// </summary>
    private IIntertactable interactable;
    /// <summary>
    /// The players movement script
    /// </summary>
    private PlayerMovement playerMovement;

    [Header("Animation")]
    [Tooltip("The animator used for the player model.")]
    [SerializeField] private Animator animator;
    [SerializeField] private string animPickUp = "";
    /// <summary>
    /// If true the interact animation is running.
    /// </summary>
    private bool interactAnimationRunning = false;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        InputManager.Instance.PlayerInput.InGame.Interact.performed += context => Interact();
    }
    private void OnDisable()
    {
        InputManager.Instance.PlayerInput.InGame.Interact.performed -= context => Interact();
    }
    private void Update()
    {
        interactable = FindInteractable();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Lets this script know when the interacting animation has finished.
    /// </summary>
    public void StopInteractAnimation()
    {
        interactAnimationRunning = false;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Finds the closest interactable object within the intaract range.
    /// </summary>
    /// <returns></returns>
    private IIntertactable FindInteractable()
    {
        Collider[] interactableArray = Physics.OverlapSphere(transform.position, interactRange, interactLayer);
        if (interactableArray.Length == 0)
        {
            if (interactable != null)
            {
                interactable.StopLookAt();
            }
            return null;
        }
        int select = 0;
        float closestDistance = float.MaxValue;
        float distance = 0.0f;
        for (int i = 0; i < interactableArray.Length; i++)
        {
            distance = Vector3.Distance(transform.position, interactableArray[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                select = i;
            }
        }

        IIntertactable newInteractable = interactableArray[select].GetComponent<IIntertactable>();
        if (newInteractable != interactable)
        {
            if (interactable != null)
            {
                interactable.StopLookAt();
            }
            if (newInteractable != null)
            {
                newInteractable.StartLookAt();
            }
        }
        return newInteractable;
    }

    /// <summary>
    /// Checks if there is something to interact with and then interacts with it.
    /// </summary>
    private void Interact()
    {
        if (interactable != null)
        {
            StartCoroutine(InteractWithObject());
        }
    }

    /// <summary>
    /// Runs the player interacting with an object over a period of time. 
    /// They move to the object, run an interact animation, run interact logic.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InteractWithObject()
    {
        InputManager.Instance.PlayerInput.InGame.Disable();

        Vector3 closestPoint = interactable.ThisCollider.ClosestPoint(transform.position);
        closestPoint.y = interactable.ThisCollider.gameObject.transform.position.y;
        Vector3 toVector = closestPoint - transform.position;
        Vector3 direction = toVector.normalized;
        float distance = toVector.magnitude - stoppingDistance;

        yield return playerMovement.MoveTo(direction, distance);

        interactAnimationRunning = true;
        animator.SetTrigger(animPickUp);

        while (interactAnimationRunning)
        {
            yield return null;
        }

        interactable.Interact();

        InputManager.Instance.PlayerInput.InGame.Enable();
    }
    #endregion
}