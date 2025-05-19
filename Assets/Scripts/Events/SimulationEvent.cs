using UnityEngine;

/// <summary>
/// Enumeration for simulation states
/// </summary>
public enum SimulationState
{
    Ready,
    Running,
    Paused,
    Completed
}

/// <summary>
/// Data structure for simulation state changes
/// </summary>
[System.Serializable]
public struct SimulationStateData
{
    /// <summary>
    /// The previous state of the simulation
    /// </summary>
    public SimulationState PreviousState;
    
    /// <summary>
    /// The new state of the simulation
    /// </summary>
    public SimulationState NewState;
    
    /// <summary>
    /// The current simulation step
    /// </summary>
    public int CurrentStep;
    
    /// <summary>
    /// The total number of steps in the simulation
    /// </summary>
    public int TotalSteps;
}

/// <summary>
/// Specialized event for simulation state changes
/// </summary>
[CreateAssetMenu(fileName = "SimulationEvent", menuName = "RPG Simulator/Events/Simulation Event")]
public class SimulationEvent : GameEvent
{
    /// <summary>
    /// The simulation state data
    /// </summary>
    public SimulationStateData StateData { get; private set; }

    /// <summary>
    /// Raises the simulation event with specific state data
    /// </summary>
    /// <param name="stateData">The simulation state data</param>
    public void Raise(SimulationStateData stateData)
    {
        StateData = stateData;
        base.Raise();
    }
}
