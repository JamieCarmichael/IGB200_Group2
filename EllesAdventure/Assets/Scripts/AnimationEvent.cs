using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Made By: Jamie Carmichael
/// Details: Calls an event.
/// </summary>
public class AnimationEvent : MonoBehaviour 
{
    #region Fields
    [Tooltip("Event to be triggered.")]
    [SerializeField] UnityEvent[] RunEvents;
    #endregion

    #region Public Methods
    /// <summary>
    /// Calls the event.
    /// </summary>
    /// <param name="eventIndex">The index of the event being called within the run events array.</param>
    public void CallEvent(int eventIndex)
    {
        RunEvents[eventIndex].Invoke();
    }
    #endregion

}