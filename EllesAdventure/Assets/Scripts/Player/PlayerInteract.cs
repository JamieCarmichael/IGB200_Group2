using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Enables the player to interact with objects with the IInteractable interface.
/// </summary>
public class PlayerInteract : MonoBehaviour 
{
    #region Fields
    [SerializeField] private bool alwaysDrop = false;

    [Header("Interact")]
    [Tooltip("The maximum interacting distance from an object.")]
    [SerializeField] private float interactRange = 1.0f;
    //[Tooltip("The distance from an intarctable object that the player will move to.")]
    //[SerializeField] private float stoppingDistance = 0.5f;
    [Tooltip("The layer for interactable objects.")]
    [SerializeField] private LayerMask interactLayer;
    /// <summary>
    /// The current interactable object.
    /// </summary>
    private IIntertactable interactable;



    private Pickup heldObject;

    public string HeldItem 
    { 
        get 
        { 
            if (heldObject == null)
            {
                return "";
            }
            return heldObject.Item; 
        } 
    }
    public Pickup HeldObject 
    { 
        get 
        { 
            return heldObject; 
        } 
    }

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

    private float playerHeight;

    private List<string> bulidableObjects = new List<string>();

    private List<string> usableItems = new List<string>() { "default" };

    private float playerRadius = 1.0f;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHeight = GetComponent<CharacterController>().height;


        playerRadius = GetComponent<CharacterController>().radius;
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
        FindInteractable();
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
        if (interactable.GetType() == typeof(Pickup) && HeldItem == "")
        {
            Pickup pickup = (Pickup)interactable;

            if (heldObject != null)
            {
                return;
            }

            if (!CanUseItemType(pickup.ItemIdentifier))
            {
                return;
            }
            interactable.Interact(HeldItem);

            heldObject = pickup;
            interactable = null;
        }
        else
        {
            interactable.Interact(HeldItem);
        }
    }

    public void RemoveHeldObject()
    {
        heldObject.gameObject.SetActive(false);
        heldObject.transform.parent = null;
        heldObject = null;
    }

    public bool CanMakeBuidlingType(string buildingType)
    {
        foreach (string item in bulidableObjects)
        {
            if (buildingType == item)
            {
                return true;
            }
        }
        return false;
    }

    public void EnableBuilding(string buildingType)
    {
        foreach (string item in bulidableObjects)
        {
            if (buildingType == item)
            {
                return;
            }
        }
        bulidableObjects.Add(buildingType);
    }
    public bool CanUseItemType(string itemType)
    {
        foreach (string item in usableItems)
        {
            if (itemType == item)
            {
                return true;
            }
        }
        return false;
    }

    public void EnableUseableItmes(string itemType)
    {
        foreach (string item in usableItems)
        {
            if (itemType == item)
            {
                return;
            }
        }
        usableItems.Add(itemType);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Finds the closest interactable object within the intaract range.
    /// </summary>
    /// <returns></returns>
    private void FindInteractable()
    {
        // Find the closest interactable object.
        Collider[] interactableArray = Physics.OverlapSphere(transform.position, interactRange, interactLayer);
        if (interactableArray.Length == 0)
        {
            if (interactable != null)
            {
                interactable.StopLookAt();
                interactable = null;
            }
            return;
        }
        if (interactableArray.Length == 1 && heldObject != null)
        {
            if (interactable != null)
            {
                interactable.StopLookAt();
                interactable = null;
            }
            return;
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

        interactable = newInteractable;
    }

    /// <summary>
    /// Checks if there is something to interact with and then interacts with it.
    /// </summary>
    private void Interact()
    {
        // Holding object but nothing to interact with.
        if (heldObject != null && interactable == null)
        {

            Collider heldObjectCollider = heldObject.GetComponent<Collider>();
            float objectLength = heldObjectCollider.bounds.extents.z;
            if (heldObjectCollider.bounds.extents.x > heldObjectCollider.bounds.extents.z)
            {
                objectLength = heldObjectCollider.bounds.extents.x;
            }

            Vector3 dropPos = transform.position + (transform.forward * objectLength) + (transform.forward * playerRadius * 2);

            if (Physics.Raycast(dropPos + (Vector3.up * playerHeight), Vector3.down, out RaycastHit hitInfo, playerHeight * 1.5f, putDownLayer, QueryTriggerInteraction.Ignore))
            {
                dropPos = hitInfo.point;
            }
            else // Not ground to drop on
            {
                Debug.DrawRay(dropPos, Vector3.up, Color.green, 1.0f);
                return;
            }

            // Only drop if area is clear
            if (!Physics.CheckBox(dropPos + (Vector3.up * heldObjectCollider.bounds.extents.y) + (Vector3.up * 0.01f), heldObjectCollider.bounds.extents, heldObject.transform.rotation))
            {
                heldObject.PutDown(dropPos);
                heldObject = null;
            }
            else if (alwaysDrop) // not clear
            {
                heldObject.PutDown(dropPos);
                heldObject = null;
            }
        }
        // Nothing held something to interact with
        else if(heldObject == null && interactable != null)
        {
            StartCoroutine(InteractWithObject());
        }
        // Holding object and something to interact with
        else if(heldObject != null && interactable != null)
        {
            if (interactable.GetType() != typeof(Pickup))
            {
                StartCoroutine(InteractWithObject());
            }
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

        // Move to closest point
        /*
        Vector3 closestPoint = interactable.GetClosestPoint(transform.position);
        yield return playerMovement.MoveTo(closestPoint, stoppingDistance);
        */

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
            ClearAnimations();
            RunInteractAction();
        }
        InputManager.Instance.PlayerInput.InGame.Enable();
    }

    private void ClearAnimations()
    {
        AnimatorControllerParameter[] a = animator.parameters;
        foreach (var item in a)
        {
            if(item.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(item.name, false);
            }
            else if (item.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(item.name);
            }
        }
    }
    #endregion
}