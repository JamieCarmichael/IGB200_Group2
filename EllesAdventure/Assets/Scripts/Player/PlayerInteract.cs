using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

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



    private Pickup heldObject;



    [SerializeField] private LayerMask putDownLayer;

    /// <summary>
    /// The players movement script
    /// </summary>
    private PlayerMovement playerMovement;

    [Header("Animation")]
    [Tooltip("The animator used for the player model.")]
    [SerializeField] private Animator animator;
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
        InputManager.Instance.PlayerInput.InGame.Interact.performed += Interact;
    }
    private void OnDisable()
    {
        InputManager.Instance.PlayerInput.InGame.Interact.performed -= Interact;
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

    public void RunInteractAction()
    {
        interactable.Interact();
        if (interactable.GetType() == typeof(Pickup))
        {
            heldObject = (Pickup)interactable;
            interactable = null;
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Finds the closest interactable object within the intaract range.
    /// </summary>
    /// <returns></returns>
    private IIntertactable FindInteractable()
    {
        // Find the closest interactable object.
        Collider[] interactableArray = Physics.OverlapSphere(transform.position, interactRange, interactLayer);
        if (interactableArray.Length == 0)
        {
            if (interactable != null)
            {
                interactable.StopLookAt();
            }
            return null;
        }
        if (interactableArray.Length == 1 && heldObject != null)
        {
            return null;
        }
        int select = 0;
        float closestDistance = float.MaxValue;
        float distance = 0.0f;
        for (int i = 0; i < interactableArray.Length; i++)
        {
            if (heldObject != null && heldObject.gameObject == interactableArray[i].gameObject)
            {
                continue;
            }
            distance = Vector3.Distance(transform.position, interactableArray[i].transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                select = i;
            }
        }

        // Sets which interactable is being looked at.
        IIntertactable newInteractable = interactableArray[select].GetComponent<IIntertactable>();
        if (newInteractable != interactable)
        {
            if (!newInteractable.Intertactable)
            {
                newInteractable = null;
            }
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
        // Holding object but nothing to interact with.
        if (heldObject != null && interactable == null)
        {
            Vector3 dropPos = transform.position + transform.forward;
            if (Physics.Raycast(dropPos + (Vector3.up * 2), Vector3.down, out RaycastHit hitInfo, 3.0f, putDownLayer, QueryTriggerInteraction.Ignore))
            {
                dropPos = hitInfo.point;
            }

            heldObject.PutDown(dropPos);
            heldObject = null;
        }
        // Nothing held something to interact with
        else if(heldObject == null && interactable != null)
        {
            StartCoroutine(InteractWithObject());
        }
        // Holding object and something to interact with
        else if(heldObject != null && interactable != null)
        { 
             // Need logic for completing task.
        }
    }

    /// <summary>
    /// Checks if there is something to interact with and then interacts with it. To be used from the Input Event.
    /// </summary>
    private void Interact(InputAction.CallbackContext obj)
    {
        Interact();
    }

    /// <summary>
    /// Runs the player interacting with an object over a period of time. 
    /// They move to the object, run an interact animation, run interact logic.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InteractWithObject()
    {
        InputManager.Instance.PlayerInput.InGame.Disable();

        Vector3 closestPoint = interactable.GetClosestPoint(transform.position);

        yield return playerMovement.MoveTo(closestPoint, stoppingDistance);

        if (interactable.InteractAminationString != string.Empty)
        {
            interactAnimationRunning = true;
            animator.SetTrigger(interactable.InteractAminationString);
            // StopInteractAnimation method needs to be called as an animation event when the animation is finishing to stop the loop.
            while (interactAnimationRunning)
            {
                yield return null;
            }
        }
        else
        {
            interactable.Interact();
        }


        InputManager.Instance.PlayerInput.InGame.Enable();
    }
    #endregion
}