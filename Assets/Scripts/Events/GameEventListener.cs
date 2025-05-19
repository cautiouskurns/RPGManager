using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component that listens for a specific GameEvent.
/// Responds by invoking a Unity Event when the GameEvent is raised.
/// </summary>
public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    [SerializeField] private GameEvent gameEvent;

    [Tooltip("Response to invoke when Event is raised.")]
    [SerializeField] private UnityEvent response;

    private void OnEnable()
    {
        // Register with the event when this component is enabled
        if (gameEvent != null)
        {
            gameEvent.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        // Unregister from the event when this component is disabled
        if (gameEvent != null)
        {
            gameEvent.UnregisterListener(this);
        }
    }

    /// <summary>
    /// Called by the GameEvent when it is raised
    /// </summary>
    public void OnEventRaised()
    {
        // Invoke the response UnityEvent
        response?.Invoke();
    }
}
