using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: When the player triggers the object then dialoge appears and an event is called.
/// </summary>
public class PlayerTriggerEvent : MonoBehaviour
{
    #region Fields
    [TextArea]
    [Tooltip("The dialoge when the trigger is activated")]
    [SerializeField] private string[] dialogue;

    [Tooltip("Event to be triggered.")]
    [SerializeField] UnityEvent RunEvent;
    #endregion

    #region Unity Call Functions
    private void Start()
    {
        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Start dialogue and run event.
        DialogueManager.Instance.DisplayDialogue(dialogue);
        RunEvent.Invoke();
        gameObject.SetActive(false);
    }

    public void EnableBuildEvent(string buildingType)
    {
        PlayerManager.Instance.PlayerInteract.EnableBuilding(buildingType);
    }

    public void EnableItemEvent(string itemType)
    {
        PlayerManager.Instance.PlayerInteract.EnableUseableItmes(itemType);
    }
    #endregion
}