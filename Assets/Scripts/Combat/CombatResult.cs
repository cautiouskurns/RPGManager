using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the result of a combat encounter.
/// </summary>
[System.Serializable]
public struct CombatResult
{
    /// <summary>
    /// Whether the player won the combat or not
    /// </summary>
    public bool PlayerVictory;
    
    /// <summary>
    /// The amount of damage dealt by the player
    /// </summary>
    public int DamageDealt;
    
    /// <summary>
    /// The amount of damage taken by the player
    /// </summary>
    public int DamageTaken;
    
    /// <summary>
    /// The XP gained from the combat
    /// </summary>
    public int XPGained;
    
    /// <summary>
    /// Log entries recording what happened during combat
    /// </summary>
    public List<string> CombatLog;
}
