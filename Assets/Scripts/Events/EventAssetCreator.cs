using UnityEngine;

/// <summary>
/// Factory class to create default event assets
/// </summary>
public static class EventAssetCreator
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void CreateEventAssets()
    {
        // Check if events already exist
        if (Resources.Load<GameEvent>("Events/OnSimulationStateChanged") == null)
        {
            // Create SimulationEvent
            SimulationEvent simulationEvent = ScriptableObject.CreateInstance<SimulationEvent>();
            
#if UNITY_EDITOR
            // Only in the editor can we create assets
            if (!System.IO.Directory.Exists("Assets/Resources/Events"))
            {
                System.IO.Directory.CreateDirectory("Assets/Resources/Events");
            }
            
            UnityEditor.AssetDatabase.CreateAsset(simulationEvent, "Assets/Resources/Events/OnSimulationStateChanged.asset");
            
            // Create CombatEvent
            CombatEvent combatEvent = ScriptableObject.CreateInstance<CombatEvent>();
            UnityEditor.AssetDatabase.CreateAsset(combatEvent, "Assets/Resources/Events/OnCombatCompleted.asset");
            
            // Create LevelUpEvent
            LevelUpEvent levelUpEvent = ScriptableObject.CreateInstance<LevelUpEvent>();
            UnityEditor.AssetDatabase.CreateAsset(levelUpEvent, "Assets/Resources/Events/OnLevelUp.asset");
            
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}
