using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Data structure for level up information
/// </summary>
[System.Serializable]
public struct LevelUpData
{
    /// <summary>
    /// The character that leveled up
    /// </summary>
    public object Character;  // Will be replaced with proper Character type once implemented
    
    /// <summary>
    /// The new level reached
    /// </summary>
    public int NewLevel;
    
    /// <summary>
    /// The stats gained in this level up
    /// </summary>
    public object StatsGained;  // Will be replaced with proper Stats type once implemented
}

/// <summary>
/// Specialized event for character level up
/// </summary>
[CreateAssetMenu(fileName = "LevelUpEvent", menuName = "RPG Simulator/Events/Level Up Event")]
public class LevelUpEvent : GameEvent
{
    /// <summary>
    /// The level up information
    /// </summary>
    public LevelUpData LevelUpData { get; private set; }

    /// <summary>
    /// Raises the level up event with specific level up data
    /// </summary>
    /// <param name="data">The level up data</param>
    public void Raise(LevelUpData data)
    {
        LevelUpData = data;
        base.Raise();
    }
}

/// <summary>
/// Unity Event type that includes LevelUpData
/// </summary>
[System.Serializable]
public class LevelUpResultEvent : UnityEvent<LevelUpData> { }

/// <summary>
/// Component that listens for a specific LevelUpEvent.
/// </summary>
public class LevelUpEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    [SerializeField] private LevelUpEvent levelUpEvent;

    [Tooltip("Response to invoke when Event is raised.")]
    [SerializeField] private LevelUpResultEvent response;

    private void OnEnable()
    {
        // Register with the event when this component is enabled
        if (levelUpEvent != null)
        {
            levelUpEvent.RegisterListener(GetComponent<GameEventListener>());
        }
    }

    private void OnDisable()
    {
        // Unregister from the event when this component is disabled
        if (levelUpEvent != null)
        {
            levelUpEvent.UnregisterListener(GetComponent<GameEventListener>());
        }
    }

    /// <summary>
    /// Called by the GameEvent when it is raised
    /// </summary>
    public void OnEventRaised()
    {
        // Invoke the response with the level up data
        response?.Invoke(levelUpEvent.LevelUpData);
    }
}
