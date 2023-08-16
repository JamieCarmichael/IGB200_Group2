using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: 
/// </summary>
public class NPCMove : MonoBehaviour 
{
    #region Fields
    [SerializeField] private GameObject walkToPosition;

    [SerializeField] private Animator animator;
    private NavMeshAgent agent;

    [SerializeField] private TalkToNPC talk;

    bool moving = false;

    [SerializeField] private float minDistance = 1.0f;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (moving)
        {
            moving = Vector3.Distance(transform.position, walkToPosition.transform.position) > minDistance;
            animator.SetBool("isWalking", moving);
        }
    }
    #endregion

    #region Public Methods
    public void Move()
    {
        moving = true;
        talk.dialogue2 = true;
        agent.SetDestination(walkToPosition.transform.position);

    }
    #endregion

    #region Private Methods
    #endregion

}