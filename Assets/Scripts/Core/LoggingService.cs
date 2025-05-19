using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Service for recording and retrieving logs
/// </summary>
public class LoggingService : MonoBehaviour
{
    // Singleton instance
    private static LoggingService _instance;
    
    public static LoggingService Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("LoggingService");
                _instance = go.AddComponent<LoggingService>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    
    // Maximum number of logs to keep
    [SerializeField] private int maxLogs = 1000;
    
    // List of log entries
    private List<LogEntry> logs = new List<LogEntry>();
    
    // Event raised when a new log is added
    [SerializeField] private UnityEvent<LogEntry> onLogAdded;
    public UnityEvent<LogEntry> OnLogAdded => onLogAdded;
    
    // Event for when logs are cleared
    [SerializeField] private UnityEvent onLogsCleared;
    public UnityEvent OnLogsCleared => onLogsCleared;
    
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
        
        if (onLogAdded == null)
            onLogAdded = new UnityEvent<LogEntry>();
            
        if (onLogsCleared == null)
            onLogsCleared = new UnityEvent();
    }
    
    /// <summary>
    /// Logs an info message
    /// </summary>
    public void LogInfo(string message, string contextData = "")
    {
        AddLog(new LogEntry(message, SimLogType.Info, contextData));
    }
    
    /// <summary>
    /// Logs a warning message
    /// </summary>
    public void LogWarning(string message, string contextData = "")
    {
        AddLog(new LogEntry(message, SimLogType.Warning, contextData));
        Debug.LogWarning(message);
    }
    
    /// <summary>
    /// Logs an error message
    /// </summary>
    public void LogError(string message, string contextData = "")
    {
        AddLog(new LogEntry(message, SimLogType.Error, contextData));
        Debug.LogError(message);
    }
    
    /// <summary>
    /// Logs a combat message
    /// </summary>
    public void LogCombat(string message, string contextData = "")
    {
        AddLog(new LogEntry(message, SimLogType.Combat, contextData));
    }
    
    /// <summary>
    /// Logs a level up message
    /// </summary>
    public void LogLevelUp(string message, string contextData = "")
    {
        AddLog(new LogEntry(message, SimLogType.LevelUp, contextData));
    }
    
    /// <summary>
    /// Logs a simulation message
    /// </summary>
    public void LogSimulation(string message, string contextData = "")
    {
        AddLog(new LogEntry(message, SimLogType.Simulation, contextData));
    }
    
    /// <summary>
    /// Adds a log entry to the list
    /// </summary>
    private void AddLog(LogEntry log)
    {
        logs.Add(log);
        
        // Limit the number of logs
        if (logs.Count > maxLogs)
        {
            logs.RemoveAt(0);
        }
        
        // Raise the event
        onLogAdded?.Invoke(log);
    }
    
    /// <summary>
    /// Clears all logs
    /// </summary>
    public void ClearLogs()
    {
        logs.Clear();
        onLogsCleared?.Invoke();
    }
    
    /// <summary>
    /// Gets all logs
    /// </summary>
    public List<LogEntry> GetAllLogs()
    {
        return new List<LogEntry>(logs);
    }
    
    /// <summary>
    /// Gets logs of a specific type
    /// </summary>
    public List<LogEntry> GetLogsByType(SimLogType type)
    {
        return logs.FindAll(log => log.Type == type);
    }
    
    /// <summary>
    /// Gets logs that contain a specific string
    /// </summary>
    public List<LogEntry> GetLogsByContent(string content)
    {
        return logs.FindAll(log => log.Message.Contains(content) || log.ContextData.Contains(content));
    }
    
    /// <summary>
    /// Gets logs from a specific time range
    /// </summary>
    public List<LogEntry> GetLogsByTimeRange(DateTime start, DateTime end)
    {
        return logs.FindAll(log => log.Timestamp >= start && log.Timestamp <= end);
    }
}
