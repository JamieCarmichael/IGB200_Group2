using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: A switch that interacts with another object.
/// </summary>
public class Lever : MonoBehaviour, IIntertactable
{
    #region Fields
    public Collider ThisCollider { get; private set; }
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
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        ThisCollider = gameObject.GetComponent<Collider>();
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods

    #endregion

    #region IIntertactable
    public void Interact()
    {
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
        nPCMove.Move();
    }

    public void StartLookAt()
    {
        //lookAtObject.SetActive(true);
    }

    public void StopLookAt()
    {
        //lookAtObject.SetActive(false);
    }
    #endregion
}