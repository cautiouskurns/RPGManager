# RPG Progression Simulator - Phase 1 Implementation
## Core Character Framework & Basic Combat

### Overview
This document outlines the implementation plan for Phase 1 of the RPG Progression Simulator. In this phase, we'll establish the core architecture and implement the fundamental systems needed for a basic but functional simulation.

**Duration:** 1-2 weeks

**Phase Objectives:**
- Establish core architecture and event system
- Create a functional character with basic stats
- Implement simple deterministic combat
- Develop minimal UI to visualize character progress

---

## Architecture Diagram

```
[SimulationManager]
    |
    ├── [EventBus] ──────────► [UI Components]
    |                               |
    ├── [CombatManager] ◄───► [CombatResolver]
    |      |
    |      ▼
    |  [CombatEvents]
    |
    ├── [LevelManager] ◄───► [ProgressionConfigSO]
    |      |
    |      ▼
    |  [LevelUpEvents]
    |
    ├── [Character] ◄───► [Stats]
    |
    └── [Enemy] ◄───────► [EnemyConfigSO]
```

---

## Required Assets

### UI Elements
- Simple UI panels (character stats, combat log, simulation controls)
- Basic icons for stats (strength, dexterity, intelligence, vitality)
- Combat log text styles
- Buttons for simulation controls (start, pause, reset)

### ScriptableObject Templates
- `ProgressionConfigSO` template for XP curve and stat gain
- `EnemyConfigSO` template for enemy types and scaling

### Testing Tools
- Simple logging display
- Parameter adjustment interface

---

## Detailed Milestones & Tasks

### Milestone 1: Project Setup and Core Infrastructure (2-3 days)

1. **Create project structure and folders**
   - Set up Unity project with appropriate folder structure
   - Create folders for Scripts, ScriptableObjects, Prefabs, UI

2. **Implement EventBus system**
   - Create `GameEvent` ScriptableObject base class
   - Implement `GameEventListener` component
   - Create concrete events: `CombatEvent`, `LevelUpEvent`, `SimulationEvent`

3. **Create SimulationManager**
   - Set up singleton pattern for global access
   - Implement simulation state (Running, Paused, Reset)
   - Create simulation tick system with configurable speed

4. **Create basic logging system**
   - Implement `LogEntry` data structure
   - Create `LoggingService` to record and retrieve logs
   - Set up simple UI display for logs

### Milestone 2: Character and Stats System (2-3 days)

1. **Create Stats structure**
   - Implement `Stats` struct with primary attributes
   - Create methods for combining and comparing stats
   - Implement stat modifier system

2. **Create Character class**
   - Implement base Character with stats, level, and XP
   - Add methods for gaining XP and leveling up
   - Create character factory for initialization

3. **Create ScriptableObject configurations**
   - Implement `ProgressionConfigSO` with level thresholds and stat growth
   - Create default progression configuration

4. **Create Enemy model**
   - Implement Enemy class with scaling stats
   - Create `EnemyConfigSO` for enemy types
   - Implement enemy generation based on character level

### Milestone 3: Combat System (2-3 days)

1. **Create CombatManager**
   - Implement combat initiation and resolution
   - Create battle state tracking
   - Add event triggers for combat phases

2. **Create CombatResolver**
   - Implement deterministic combat calculations
   - Create hit/miss logic based on stats
   - Implement damage calculation

3. **Set up CombatEvents**
   - Create event structure for combat results
   - Implement event raising on combat completion
   - Add listeners for UI updates

4. **Link combat to simulation loop**
   - Connect CombatManager to SimulationManager
   - Implement enemy generation per simulation cycle
   - Create combat logging

### Milestone 4: Experience and Leveling System (1-2 days)

1. **Create LevelManager**
   - Implement XP award system
   - Create level-up detection
   - Add stat increase logic on level-up

2. **Implement XP calculation**
   - Create XP rewards based on enemy level
   - Implement XP curve via ProgressionConfigSO
   - Add level threshold calculation

3. **Connect leveling to combat**
   - Add XP rewards to combat resolution
   - Implement automatic level-up after combat
   - Create level-up events

### Milestone 5: User Interface (2-3 days)

1. **Create character stats display**
   - Implement StatsView component
   - Add real-time update of character stats
   - Create visual feedback for stat changes

2. **Create combat log UI**
   - Implement scrolling combat log
   - Add formatting for different log types
   - Create filtering options

3. **Create simulation controls**
   - Implement start/pause/reset buttons
   - Add simulation speed control
   - Create simple configuration panel

4. **Connect UI to events**
   - Hook up UI updates to EventBus
   - Implement event listeners for all UI elements
   - Add visual feedback for events

### Milestone 6: Testing and Refinement (2-3 days)

1. **Create testing tools**
   - Implement parameter adjustment interface
   - Add real-time stat monitoring
   - Create simulation summary view

2. **Perform initial balancing**
   - Test XP curve and leveling speed
   - Adjust combat formulas for appropriate difficulty
   - Balance enemy scaling

3. **Debug and optimize**
   - Fix any functional issues
   - Optimize performance bottlenecks
   - Ensure clean event handling

4. **Documentation**
   - Document class relationships
   - Create usage guide for ScriptableObjects
   - Document testing procedures

---

## Core Classes

### SimulationManager

```csharp
/// <summary>
/// Core orchestrator for the RPG simulation.
/// </summary>
public class SimulationManager : MonoBehaviour
{
    // Configuration
    [SerializeField] private float simulationSpeed = 1.0f;
    [SerializeField] private int maxSimulationSteps = 100;
    
    // References
    [SerializeField] private ProgressionConfigSO progressionConfig;
    [SerializeField] private EnemyConfigSO enemyConfig;
    
    // State
    private Character playerCharacter;
    private SimulationState currentState;
    private int currentStep;
    
    // Events
    [SerializeField] private GameEvent onSimulationStart;
    [SerializeField] private GameEvent onSimulationPause;
    [SerializeField] private GameEvent onSimulationComplete;
    
    // Core Methods
    public void StartSimulation();
    public void PauseSimulation();
    public void ResetSimulation();
    private void RunSimulationStep();
    private Enemy GenerateEnemy(int playerLevel);
    
    // Initialization
    private void Initialize();
    private Character CreatePlayerCharacter();
}
```

### EventBus (ScriptableObject-based)

```csharp
/// <summary>
/// Base class for all game events in the event system.
/// </summary>
[CreateAssetMenu(fileName = "GameEvent", menuName = "RPG Simulator/Events/Game Event")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();
    
    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised();
    }
    
    public void RegisterListener(GameEventListener listener);
    public void UnregisterListener(GameEventListener listener);
}

/// <summary>
/// Component that listens for a specific GameEvent.
/// </summary>
public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private UnityEvent response;
    
    private void OnEnable();
    private void OnDisable();
    public void OnEventRaised();
}

/// <summary>
/// Specialized event for combat results.
/// </summary>
[CreateAssetMenu(fileName = "CombatEvent", menuName = "RPG Simulator/Events/Combat Event")]
public class CombatEvent : GameEvent
{
    public CombatResult CombatResult { get; private set; }
    
    public void Raise(CombatResult result);
}
```

### Character and Stats

```csharp
/// <summary>
/// Struct representing character statistics.
/// </summary>
[System.Serializable]
public struct Stats
{
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int Vitality;
    
    // Methods
    public static Stats operator +(Stats a, Stats b);
    public static Stats operator -(Stats a, Stats b);
    public int CalculateAttackPower();
    public int CalculateDefense();
    public int CalculateHealth();
}

/// <summary>
/// Core class representing a player character.
/// </summary>
public class Character
{
    // Core stats
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int CurrentXP { get; private set; }
    public Stats BaseStats { get; private set; }
    
    // Derived stats
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int AttackPower { get; private set; }
    public int Defense { get; private set; }
    
    // Methods
    public void GainXP(int amount);
    public bool CheckLevelUp();
    public void LevelUp();
    public void ApplyStatIncrease(Stats increase);
    public void RecalculateDerivedStats();
    public CombatResult SimulateCombat(Enemy enemy);
}
```

### Combat System

```csharp
/// <summary>
/// Manages combat encounters and resolution.
/// </summary>
public class CombatManager : MonoBehaviour
{
    // References
    [SerializeField] private CombatEvent onCombatComplete;
    [SerializeField] private LoggingService loggingService;
    
    // Core methods
    public CombatResult ResolveCombat(Character character, Enemy enemy);
    private void LogCombatResults(CombatResult result);
    public void ApplyCombatRewards(Character character, CombatResult result);
}

/// <summary>
/// Pure logic class for resolving combat math.
/// </summary>
public class CombatResolver
{
    public static CombatResult ResolveCombat(Stats attackerStats, Stats defenderStats);
    private static int CalculateDamage(int attackPower, int defense);
    private static bool DetermineHit(int attackerDexterity, int defenderDexterity);
}

/// <summary>
/// Stores the result of a combat encounter.
/// </summary>
public struct CombatResult
{
    public bool PlayerVictory;
    public int DamageDealt;
    public int DamageTaken;
    public int XPGained;
    public List<string> CombatLog;
}
```

### LevelManager and Progression

```csharp
/// <summary>
/// Configuration for character progression.
/// </summary>
[CreateAssetMenu(fileName = "ProgressionConfig", menuName = "RPG Simulator/Progression Config")]
public class ProgressionConfigSO : ScriptableObject
{
    [SerializeField] private AnimationCurve xpCurve;
    [SerializeField] private int baseXPRequirement = 100;
    [SerializeField] private int xpMultiplier = 150;
    [SerializeField] private Stats statGainPerLevel;
    
    public int GetXPForLevel(int level);
    public Stats GetStatGainForLevel(int level);
}

/// <summary>
/// Manages character leveling and progression.
/// </summary>
public class LevelManager : MonoBehaviour
{
    // References
    [SerializeField] private ProgressionConfigSO progressionConfig;
    [SerializeField] private GameEvent onLevelUp;
    
    // Methods
    public int CalculateXPReward(int enemyLevel, int characterLevel);
    public bool CheckLevelUp(Character character);
    public void ApplyLevelUp(Character character);
}
```

---

## Core Game Loop for Phase 1

The core gameplay loop in Phase 1 is straightforward but provides a complete simulation cycle:

1. **Initialization:**
   - Player character is created with base stats
   - Initial enemy is generated scaled to player level

2. **Simulation Loop:**
   - Combat is resolved deterministically based on stats
   - Combat results are logged and displayed
   - XP is awarded for victory
   - Level-up check is performed
   - If level up occurs, stats increase
   - New enemy is generated with appropriate scaling
   - Cycle repeats

3. **User Interaction:**
   - Start/pause/reset simulation controls
   - Adjust simulation speed
   - View character stats in real-time
   - Read combat log for outcomes
   - (Optional) Configure initial character stats

4. **Feedback:**
   - Visual feedback on level progression
   - Combat log shows detailed outcomes
   - Stat changes are displayed after level-ups

The gameplay at this stage is entirely observational - the player sets up the simulation and watches it run. The key experience is seeing how the character progresses through levels and becomes more powerful, as evidenced by more consistent victories against higher-level enemies.

---

## Testing Focus

The primary testing focus for Phase 1 is to verify:

1. **Core Loop Functionality:** Ensure the simulation can run through multiple cycles without errors
2. **Leveling Curve:** Verify the XP and level progression feels appropriate and steady
3. **Combat Balance:** Check that combat outcomes make sense based on relative character/enemy stats
4. **Event System:** Confirm that events are properly raised and handled throughout the system
5. **Performance:** Make sure the simulation can run efficiently even for longer sessions

---

## Next Steps after Phase 1

Upon successful completion of Phase 1, we'll move to Phase 2, where we'll implement:
- Inventory system
- Basic equipment with stat bonuses
- Equipment slots
- Equipment comparison logic

These additions will make the simulation more dynamic by introducing gear progression alongside character leveling.
