using UnityEngine;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: When the player triggers the object a task is done.
/// </summary>
public class PlayerTriggerLocation : MonoBehaviour
{
    #region Fields
    private SubTask subtask;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.enabled = false;
        }
        if (subtask == null)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        subtask.DoSubtask();
        subtask = null;
        gameObject.SetActive(false);
    }

    public void SetCurrentTask(SubTask newSubtask)
    {
        subtask = newSubtask;
        gameObject.SetActive(true);
    }
    #endregion
}