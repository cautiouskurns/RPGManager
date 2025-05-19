using UnityEngine;

/// <summary>
/// Initializes all core systems when the application starts
/// </summary>
public class CoreSystemsInitializer : MonoBehaviour
{
    [SerializeField] private bool initializeOnAwake = true;
    
    private void Awake()
    {
        if (initializeOnAwake)
        {
            InitializeSystems();
        }
    }
    
    /// <summary>
    /// Initializes all core systems
    /// </summary>
    public void InitializeSystems()
    {
        // Initialize logging service first
        LoggingService logger = LoggingService.Instance;
        logger.LogInfo("Initializing core systems");
        
        // Initialize simulation manager
        SimulationManager simulation = SimulationManager.Instance;
        logger.LogInfo("Core systems initialized");
    }
}
