using UnityEngine;

/// <summary>
/// Initializes the EventBus at application start
/// </summary>
public static class EventBusInitializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        // This will create the EventBus singleton
        Debug.Log("Initializing EventBus...");
        EventBus eventBus = EventBus.Instance;
    }
}
