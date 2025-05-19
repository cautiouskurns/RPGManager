using System;
using UnityEngine;

/// <summary>
/// Represents a single log entry in the simulation
/// </summary>
[System.Serializable]
public struct LogEntry
{
    /// <summary>
    /// The timestamp when this log was created
    /// </summary>
    [SerializeField]
    public DateTime Timestamp;
    
    /// <summary>
    /// The log message
    /// </summary>
    [SerializeField]
    public string Message;
    
    /// <summary>
    /// The type of log entry (info, warning, error, combat, level, etc.)
    /// </summary>
    [SerializeField]
    public SimLogType Type;
    
    /// <summary>
    /// Optional context data associated with this log
    /// </summary>
    [SerializeField]
    public string ContextData;

    /// <summary>
    /// Creates a new log entry
    /// </summary>
    /// <param name="message">The log message</param>
    /// <param name="type">The type of log</param>
    /// <param name="contextData">Optional context data</param>
    public LogEntry(string message, SimLogType type = SimLogType.Info, string contextData = "")
    {
        Timestamp = DateTime.Now;
        Message = message;
        Type = type;
        ContextData = contextData;
    }
    
    /// <summary>
    /// Gets a formatted string representation of this log entry
    /// </summary>
    public string FormattedMessage
    {
        get
        {
            string prefix = $"[{Timestamp.ToString("HH:mm:ss")}] [{Type}] ";
            return prefix + Message;
        }
    }
    
    /// <summary>
    /// Gets the color associated with this log type
    /// </summary>
    public Color GetColor()
    {
        switch (Type)
        {
            case SimLogType.Error:
                return Color.red;
            case SimLogType.Warning:
                return Color.yellow;
            case SimLogType.Combat:
                return new Color(1f, 0.5f, 0.5f); // Light red
            case SimLogType.LevelUp:
                return new Color(0.5f, 1f, 0.5f); // Light green
            case SimLogType.Simulation:
                return new Color(0.5f, 0.5f, 1f); // Light blue
            case SimLogType.Info:
            default:
                return Color.white;
        }
    }
}

/// <summary>
/// Types of log entries in the simulation
/// </summary>
public enum SimLogType
{
    Info,
    Warning,
    Error,
    Combat,
    LevelUp,
    Simulation
}
