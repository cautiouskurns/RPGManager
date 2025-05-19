using UnityEngine;
using System.Collections;

/// <summary>
/// Core orchestrator for the RPG simulation.
/// Handles simulation state and timing.
/// </summary>
public class SimulationManager : MonoBehaviour
{
    // Singleton instance
    private static SimulationManager _instance;
    
    public static SimulationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("SimulationManager");
                _instance = go.AddComponent<SimulationManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    
    // Configuration
    [Header("Simulation Settings")]
    [Tooltip("How fast the simulation runs")]
    [SerializeField] private float simulationSpeed = 1.0f;
    
    [Tooltip("Time between simulation ticks in seconds")]
    [SerializeField] private float tickInterval = 1.0f;
    
    [Tooltip("Maximum number of steps the simulation will run")]
    [SerializeField] private int maxSimulationSteps = 100;
    
    // References
    [Header("Events")]
    [SerializeField] private SimulationEvent onSimulationStateChanged;
    
    // State
    private SimulationState currentState = SimulationState.Ready;
    private int currentStep = 0;
    private Coroutine simulationCoroutine;
    
    // Properties
    public SimulationState CurrentState => currentState;
    public int CurrentStep => currentStep;
    public float SimulationSpeed 
    {
        get => simulationSpeed;
        set => simulationSpeed = Mathf.Clamp(value, 0.1f, 10f);
    }
    
    private void Awake()
    {
        // Ensure singleton behavior
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Initialize
        Initialize();
    }
    
    /// <summary>
    /// Initializes the simulation
    /// </summary>
    private void Initialize()
    {
        // Ensure we have the events
        if (onSimulationStateChanged == null)
        {
            onSimulationStateChanged = Resources.Load<SimulationEvent>("Events/OnSimulationStateChanged");
            if (onSimulationStateChanged == null)
            {
                LoggingService.Instance.LogError("Failed to load OnSimulationStateChanged event");
            }
        }
        
        // Log the initialization
        LoggingService.Instance.LogSimulation("Simulation manager initialized");
        
        // Set initial state
        SetState(SimulationState.Ready);
    }
    
    /// <summary>
    /// Starts the simulation
    /// </summary>
    public void StartSimulation()
    {
        if (currentState == SimulationState.Running)
            return;
            
        LoggingService.Instance.LogSimulation("Starting simulation");
        
        // Start the simulation coroutine
        if (simulationCoroutine != null)
        {
            StopCoroutine(simulationCoroutine);
        }
        
        simulationCoroutine = StartCoroutine(RunSimulation());
        
        // Set the state
        SetState(SimulationState.Running);
    }
    
    /// <summary>
    /// Pauses the simulation
    /// </summary>
    public void PauseSimulation()
    {
        if (currentState != SimulationState.Running)
            return;
            
        LoggingService.Instance.LogSimulation("Pausing simulation");
        
        // Stop the simulation coroutine
        if (simulationCoroutine != null)
        {
            StopCoroutine(simulationCoroutine);
            simulationCoroutine = null;
        }
        
        // Set the state
        SetState(SimulationState.Paused);
    }
    
    /// <summary>
    /// Resets the simulation
    /// </summary>
    public void ResetSimulation()
    {
        LoggingService.Instance.LogSimulation("Resetting simulation");
        
        // Stop the simulation coroutine
        if (simulationCoroutine != null)
        {
            StopCoroutine(simulationCoroutine);
            simulationCoroutine = null;
        }
        
        // Reset state
        currentStep = 0;
        
        // Set the state
        SetState(SimulationState.Ready);
    }
    
    /// <summary>
    /// Sets the simulation state and raises events
    /// </summary>
    private void SetState(SimulationState newState)
    {
        // If state hasn't changed, do nothing
        if (newState == currentState)
            return;
            
        SimulationState previousState = currentState;
        currentState = newState;
        
        // Create state data
        SimulationStateData stateData = new SimulationStateData
        {
            PreviousState = previousState,
            NewState = currentState,
            CurrentStep = currentStep,
            TotalSteps = maxSimulationSteps
        };
        
        // Raise the event
        if (onSimulationStateChanged != null)
        {
            onSimulationStateChanged.Raise(stateData);
        }
        else
        {
            LoggingService.Instance.LogWarning("onSimulationStateChanged event is null");
        }
    }
    
    /// <summary>
    /// Coroutine that runs the simulation
    /// </summary>
    private IEnumerator RunSimulation()
    {
        while (currentStep < maxSimulationSteps && currentState == SimulationState.Running)
        {
            // Calculate the actual wait time based on simulation speed
            float actualTickInterval = tickInterval / simulationSpeed;
            
            // Wait for the next tick
            yield return new WaitForSeconds(actualTickInterval);
            
            // Run the simulation step
            RunSimulationStep();
            
            // Increment the step
            currentStep++;
            
            // Check if we've reached the end
            if (currentStep >= maxSimulationSteps)
            {
                LoggingService.Instance.LogSimulation($"Simulation completed after {currentStep} steps");
                SetState(SimulationState.Completed);
            }
        }
    }
    
    /// <summary>
    /// Runs a single simulation step
    /// </summary>
    private void RunSimulationStep()
    {
        // Log the step
        LoggingService.Instance.LogSimulation($"Running simulation step {currentStep + 1}/{maxSimulationSteps}");
        
        // TODO: Implement simulation step logic
        // - Generate enemy
        // - Run combat
        // - Handle rewards
        // - Level up if needed
    }
    
    /// <summary>
    /// Generates an enemy based on the character level
    /// </summary>
    private void GenerateEnemy(int playerLevel)
    {
        // TODO: Implement enemy generation
        LoggingService.Instance.LogSimulation($"Generating enemy for player level {playerLevel}");
    }
    
    /// <summary>
    /// Creates the player character
    /// </summary>
    private void CreatePlayerCharacter()
    {
        // TODO: Implement character creation
        LoggingService.Instance.LogSimulation("Creating player character");
    }
}
