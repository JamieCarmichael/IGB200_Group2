using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A switch that interacts with another object.
/// </summary>
public class Lever : MonoBehaviour, IIntertactable
{
    #region Fields
    private Collider thisCollider;

    public string InteractAminationString
    {
        get
        {
            return interactAminationString;
        }
    }

    [SerializeField] private string interactAminationString;


    public bool Intertactable
    {
        get
        {
            return intertactable;
        }
    }

    private bool intertactable = true;

    [Tooltip("The object that indicates when this object is being looked at.")]
    private GameObject lookAtObject;

    [SerializeField] private GameObject gate;

    [SerializeField] private float openTime = 1.0f;

    [SerializeField] private NPCMove nPCMove;

    [SerializeField] private string requiredItem;

    [SerializeField] private int numberOfItmesNeeded; 

    [SerializeField] private bool useRequiredItem = false;

    #endregion

    #region Unity Call Functions
    private void Start()
    {
        thisCollider = gameObject.GetComponent<Collider>();
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods

    #endregion

    #region IIntertactable
    public void Interact()
    {
        if (requiredItem != null)
        {
            if (!PlayerManager.Instance.UseItem(requiredItem, numberOfItmesNeeded, useRequiredItem))
            {
                return;
            }
        }

        gameObject.GetComponent<Animator>().SetTrigger("Open");
        Invoke("OpenGate", openTime);
    }

    private void OpenGate()
    {
        gate.GetComponent<Animator>().SetTrigger("Open");
        Invoke("MoveNPC", openTime);
    }

    private void MoveNPC()
    {
        if (nPCMove != null)
        {
            nPCMove.Move();
        }
    }

    public void StartLookAt()
    {
        //lookAtObject.SetActive(true);
    }

    public void StopLookAt()
    {
        //lookAtObject.SetActive(false);
    }

    public Vector3 GetClosestPoint(Vector3 playerPos)
    {
        Vector3 closestPoint = thisCollider.ClosestPoint(playerPos);
        closestPoint.y = transform.position.y;
        return closestPoint;
    }
    #endregion
}