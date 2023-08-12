using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Calls an event.
/// </summary>
public class TriggerEvent : MonoBehaviour 
{
    #region Fields
    [Tooltip("Event to be triggered.")]
    [SerializeField] UnityEvent RunEvent;
    #endregion

    #region Public Methods
    /// <summary>
    /// Calls the event.
    /// </summary>
    public void CallEvent()
    {
        RunEvent.Invoke();
    }
    #endregion

}