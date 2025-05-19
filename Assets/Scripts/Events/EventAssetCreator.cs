using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Factory class to create default event assets
/// </summary>
public static class EventAssetCreator
{
#if UNITY_EDITOR
    [MenuItem("RPG Simulator/Create Default Events")]
    public static void CreateEventAssetsMenuOption()
    {
        Debug.Log("Creating event assets from menu...");
        CreateEventAssetsInternal();
    }
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void CreateEventAssets()
    {
        Debug.Log("Checking if event assets need to be created...");
#if UNITY_EDITOR
        // Only create assets in the Unity Editor
        if (!File.Exists("Assets/Resources/Events/OnSimulationStateChanged.asset"))
        {
            CreateEventAssetsInternal();
        }
        else
        {
            Debug.Log("Event assets already exist.");
        }
#endif
    }
    
#if UNITY_EDITOR
    private static void CreateEventAssetsInternal()
    {
        Debug.Log("Creating event assets...");
        
        // Ensure the directory exists
        string directory = "Assets/Resources/Events";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        // Create SimulationEvent
        Debug.Log("Creating SimulationEvent...");
        SimulationEvent simulationEvent = ScriptableObject.CreateInstance<SimulationEvent>();
        AssetDatabase.CreateAsset(simulationEvent, "Assets/Resources/Events/OnSimulationStateChanged.asset");
        
        // Create CombatEvent
        Debug.Log("Creating CombatEvent...");
        CombatEvent combatEvent = ScriptableObject.CreateInstance<CombatEvent>();
        AssetDatabase.CreateAsset(combatEvent, "Assets/Resources/Events/OnCombatCompleted.asset");
        
        // Create LevelUpEvent
        Debug.Log("Creating LevelUpEvent...");
        LevelUpEvent levelUpEvent = ScriptableObject.CreateInstance<LevelUpEvent>();
        AssetDatabase.CreateAsset(levelUpEvent, "Assets/Resources/Events/OnLevelUp.asset");
        
        // Create simple GameEvent
        Debug.Log("Creating basic GameEvent...");
        GameEvent gameEvent = ScriptableObject.CreateInstance<GameEvent>();
        AssetDatabase.CreateAsset(gameEvent, "Assets/Resources/Events/OnSimulationReset.asset");
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("Event assets created successfully!");
    }
#endif
}
