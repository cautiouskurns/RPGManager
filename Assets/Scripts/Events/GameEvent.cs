using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all game events in the event system.
/// Acts as a scriptable object that can notify all its listeners when raised.
/// </summary>
[CreateAssetMenu(fileName = "GameEvent", menuName = "RPG Simulator/Events/Game Event")]
public class GameEvent : ScriptableObject
{
    // List of listeners that will be notified when this event is raised
    private readonly List<GameEventListener> listeners = new List<GameEventListener>();

    /// <summary>
    /// Raises the event, notifying all listeners
    /// </summary>
    public virtual void Raise()
    {
        // Iterate backwards in case listeners remove themselves during execution
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    /// <summary>
    /// Registers a listener to this event
    /// </summary>
    /// <param name="listener">The listener to register</param>
    public void RegisterListener(GameEventListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    /// <summary>
    /// Unregisters a listener from this event
    /// </summary>
    /// <param name="listener">The listener to unregister</param>
    public void UnregisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}
