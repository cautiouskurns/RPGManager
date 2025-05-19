using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central event bus for the application.
/// Provides a central point for raising events and subscribing to them.
/// Works alongside the ScriptableObject-based event system.
/// </summary>
public class EventBus : MonoBehaviour
{
    // Singleton instance
    private static EventBus _instance;

    public static EventBus Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("EventBus");
                _instance = go.AddComponent<EventBus>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // Dictionary to store event types and their callbacks
    private Dictionary<Type, List<object>> eventCallbacks = new Dictionary<Type, List<object>>();

    // Dictionary to store available ScriptableObject events
    private Dictionary<string, GameEvent> scriptableEvents = new Dictionary<string, GameEvent>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Load all scriptable events at startup
        LoadScriptableEvents();
    }

    /// <summary>
    /// Loads all ScriptableObject events from Resources
    /// </summary>
    private void LoadScriptableEvents()
    {
        GameEvent[] events = Resources.LoadAll<GameEvent>("Events");
        
        foreach (var evt in events)
        {
            scriptableEvents[evt.name] = evt;
            Debug.Log($"Loaded event: {evt.name}");
        }
    }

    /// <summary>
    /// Get a ScriptableObject event by name
    /// </summary>
    public T GetScriptableEvent<T>(string eventName) where T : GameEvent
    {
        if (scriptableEvents.TryGetValue(eventName, out GameEvent evt))
        {
            return evt as T;
        }
        
        Debug.LogWarning($"ScriptableObject event '{eventName}' not found!");
        return null;
    }

    /// <summary>
    /// Raise a ScriptableObject event by name
    /// </summary>
    public void RaiseScriptableEvent(string eventName)
    {
        if (scriptableEvents.TryGetValue(eventName, out GameEvent evt))
        {
            evt.Raise();
        }
        else
        {
            Debug.LogWarning($"ScriptableObject event '{eventName}' not found!");
        }
    }

    /// <summary>
    /// Raise a CombatEvent with combat result data
    /// </summary>
    public void RaiseCombatEvent(string eventName, CombatResult result)
    {
        if (scriptableEvents.TryGetValue(eventName, out GameEvent evt) && evt is CombatEvent combatEvent)
        {
            combatEvent.Raise(result);
        }
        else
        {
            Debug.LogWarning($"CombatEvent '{eventName}' not found or wrong type!");
        }
    }

    /// <summary>
    /// Raise a LevelUpEvent with level up data
    /// </summary>
    public void RaiseLevelUpEvent(string eventName, LevelUpData data)
    {
        if (scriptableEvents.TryGetValue(eventName, out GameEvent evt) && evt is LevelUpEvent levelUpEvent)
        {
            levelUpEvent.Raise(data);
        }
        else
        {
            Debug.LogWarning($"LevelUpEvent '{eventName}' not found or wrong type!");
        }
    }

    /// <summary>
    /// Raise a SimulationEvent with simulation state data
    /// </summary>
    public void RaiseSimulationEvent(string eventName, SimulationStateData data)
    {
        if (scriptableEvents.TryGetValue(eventName, out GameEvent evt) && evt is SimulationEvent simEvent)
        {
            simEvent.Raise(data);
        }
        else
        {
            Debug.LogWarning($"SimulationEvent '{eventName}' not found or wrong type!");
        }
    }

    // For code-based event subscription (in addition to the ScriptableObject events)
    
    /// <summary>
    /// Subscribe to an event type
    /// </summary>
    public void Subscribe<T>(Action<T> callback)
    {
        Type eventType = typeof(T);
        
        if (!eventCallbacks.ContainsKey(eventType))
        {
            eventCallbacks[eventType] = new List<object>();
        }
        
        eventCallbacks[eventType].Add(callback);
    }

    /// <summary>
    /// Unsubscribe from an event type
    /// </summary>
    public void Unsubscribe<T>(Action<T> callback)
    {
        Type eventType = typeof(T);
        
        if (eventCallbacks.ContainsKey(eventType))
        {
            eventCallbacks[eventType].Remove(callback);
        }
    }

    /// <summary>
    /// Publish an event of type T
    /// </summary>
    public void Publish<T>(T eventData)
    {
        Type eventType = typeof(T);
        
        if (eventCallbacks.ContainsKey(eventType))
        {
            foreach (var callback in eventCallbacks[eventType])
            {
                if (callback is Action<T> typedCallback)
                {
                    typedCallback(eventData);
                }
            }
        }
    }
}
