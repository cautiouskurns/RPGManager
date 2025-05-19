using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Specialized event for combat results.
/// </summary>
[CreateAssetMenu(fileName = "CombatEvent", menuName = "RPG Simulator/Events/Combat Event")]
public class CombatEvent : GameEvent
{
    /// <summary>
    /// The result of the combat
    /// </summary>
    [SerializeField] private CombatResult combatResult;
    
    public CombatResult CombatResult
    {
        get { return combatResult; }
        private set { combatResult = value; }
    }

    /// <summary>
    /// Raises the combat event with specific combat result data
    /// </summary>
    /// <param name="result">The combat result data</param>
    public void Raise(CombatResult result)
    {
        CombatResult = result;
        base.Raise();
    }
}

/// <summary>
/// Unity Event type that includes CombatResult data
/// </summary>
[System.Serializable]
public class CombatResultEvent : UnityEvent<CombatResult> { }

/// <summary>
/// Component that listens for a specific CombatEvent.
/// </summary>
public class CombatEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    [SerializeField] private CombatEvent combatEvent;

    [Tooltip("Response to invoke when Event is raised.")]
    [SerializeField] private CombatResultEvent response;

    private void OnEnable()
    {
        // Register with the event when this component is enabled
        if (combatEvent != null)
        {
            combatEvent.RegisterListener(GetComponent<GameEventListener>());
        }
    }

    private void OnDisable()
    {
        // Unregister from the event when this component is disabled
        if (combatEvent != null)
        {
            combatEvent.UnregisterListener(GetComponent<GameEventListener>());
        }
    }

    /// <summary>
    /// Called by the GameEvent when it is raised
    /// </summary>
    public void OnEventRaised()
    {
        // Invoke the response with the combat result data
        response?.Invoke(combatEvent.CombatResult);
    }
}
