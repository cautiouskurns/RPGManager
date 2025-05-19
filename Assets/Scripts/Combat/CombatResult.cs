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
    [SerializeField]
    public bool PlayerVictory;
    
    /// <summary>
    /// The amount of damage dealt by the player
    /// </summary>
    [SerializeField]
    public int DamageDealt;
    
    /// <summary>
    /// The amount of damage taken by the player
    /// </summary>
    [SerializeField]
    public int DamageTaken;
    
    /// <summary>
    /// The XP gained from the combat
    /// </summary>
    [SerializeField]
    public int XPGained;
    
    /// <summary>
    /// Log entries recording what happened during combat
    /// </summary>
    [SerializeField]
    public List<string> CombatLog;
}
