using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Displays logs in the UI
/// </summary>
public class LogDisplay : MonoBehaviour
{
    [Tooltip("Prefab for a log entry UI element")]
    [SerializeField] private GameObject logEntryPrefab;
    
    [Tooltip("Content transform where log entries will be parented")]
    [SerializeField] private RectTransform logContent;
    
    [Tooltip("How many log entries to display at once")]
    [SerializeField] private int maxDisplayedLogs = 100;
    
    [Tooltip("Dropdown for filtering log types")]
    [SerializeField] private TMP_Dropdown filterDropdown;
    
    [Tooltip("Input field for filtering by content")]
    [SerializeField] private TMP_InputField filterInput;
    
    [Tooltip("Whether to auto-scroll to the bottom")]
    [SerializeField] private bool autoScroll = true;
    
    [Tooltip("Scroll rect component")]
    [SerializeField] private ScrollRect scrollRect;
    
    // List of instantiated log UI elements
    private List<GameObject> logEntryObjects = new List<GameObject>();
    
    // Currently applied filters
    private SimLogType? typeFilter = null;
    private string contentFilter = "";
    
    private void Awake()
    {
        // Make sure we have required components
        if (logContent == null)
        {
            logContent = GetComponentInChildren<RectTransform>();
        }
        
        if (scrollRect == null)
        {
            scrollRect = GetComponentInChildren<ScrollRect>();
        }
        
        // Set up the dropdown options if available
        if (filterDropdown != null)
        {
            filterDropdown.ClearOptions();
            
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("All"),
                new TMP_Dropdown.OptionData("Info"),
                new TMP_Dropdown.OptionData("Warning"),
                new TMP_Dropdown.OptionData("Error"),
                new TMP_Dropdown.OptionData("Combat"),
                new TMP_Dropdown.OptionData("Level Up"),
                new TMP_Dropdown.OptionData("Simulation")
            };
            
            filterDropdown.AddOptions(options);
            filterDropdown.onValueChanged.AddListener(OnFilterTypeChanged);
        }
        
        // Set up the input field if available
        if (filterInput != null)
        {
            filterInput.onValueChanged.AddListener(OnFilterContentChanged);
        }
    }
    
    private void OnEnable()
    {
        // Subscribe to log events
        LoggingService.Instance.OnLogAdded.AddListener(OnLogAdded);
        LoggingService.Instance.OnLogsCleared.AddListener(OnLogsCleared);
        
        // Display existing logs
        RefreshLogs();
    }
    
    private void OnDisable()
    {
        // Unsubscribe from log events
        if (LoggingService.Instance != null)
        {
            LoggingService.Instance.OnLogAdded.RemoveListener(OnLogAdded);
            LoggingService.Instance.OnLogsCleared.RemoveListener(OnLogsCleared);
        }
    }
    
    /// <summary>
    /// Refreshes the log display with all logs
    /// </summary>
    public void RefreshLogs()
    {
        // Clear existing log entries
        ClearLogEntries();
        
        // Get filtered logs
        List<LogEntry> logs = GetFilteredLogs();
        
        // Display logs
        foreach (LogEntry log in logs)
        {
            CreateLogEntryUI(log);
        }
        
        // Scroll to bottom if auto-scroll is enabled
        if (autoScroll && scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.normalizedPosition = new Vector2(0, 0);
        }
    }
    
    /// <summary>
    /// Handler for when a new log is added
    /// </summary>
    private void OnLogAdded(LogEntry log)
    {
        // Check if the log passes our filters
        if (PassesFilter(log))
        {
            // Create the UI for the log
            CreateLogEntryUI(log);
            
            // Remove oldest log if we have too many
            if (logEntryObjects.Count > maxDisplayedLogs)
            {
                GameObject oldestLog = logEntryObjects[0];
                logEntryObjects.RemoveAt(0);
                Destroy(oldestLog);
            }
            
            // Scroll to bottom if auto-scroll is enabled
            if (autoScroll && scrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                scrollRect.normalizedPosition = new Vector2(0, 0);
            }
        }
    }
    
    /// <summary>
    /// Handler for when logs are cleared
    /// </summary>
    private void OnLogsCleared()
    {
        ClearLogEntries();
    }
    
    /// <summary>
    /// Creates a UI element for a log entry
    /// </summary>
    private void CreateLogEntryUI(LogEntry log)
    {
        if (logEntryPrefab == null || logContent == null)
            return;
            
        // Instantiate the prefab
        GameObject logEntryObject = Instantiate(logEntryPrefab, logContent);
        
        // Get the text component
        TextMeshProUGUI textComponent = logEntryObject.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = log.FormattedMessage;
            textComponent.color = log.GetColor();
        }
        
        // Add to our list
        logEntryObjects.Add(logEntryObject);
    }
    
    /// <summary>
    /// Clears all log entry UI elements
    /// </summary>
    private void ClearLogEntries()
    {
        foreach (GameObject logEntryObject in logEntryObjects)
        {
            Destroy(logEntryObject);
        }
        
        logEntryObjects.Clear();
    }
    
    /// <summary>
    /// Gets logs that match the current filters
    /// </summary>
    private List<LogEntry> GetFilteredLogs()
    {
        List<LogEntry> allLogs = LoggingService.Instance.GetAllLogs();
        List<LogEntry> filteredLogs = new List<LogEntry>();
        
        foreach (LogEntry log in allLogs)
        {
            if (PassesFilter(log))
            {
                filteredLogs.Add(log);
            }
        }
        
        return filteredLogs;
    }
    
    /// <summary>
    /// Checks if a log entry passes the current filters
    /// </summary>
    private bool PassesFilter(LogEntry log)
    {
        // Type filter
        if (typeFilter.HasValue && log.Type != typeFilter.Value)
            return false;
            
        // Content filter
        if (!string.IsNullOrEmpty(contentFilter) && 
            !log.Message.ToLower().Contains(contentFilter.ToLower()) && 
            !log.ContextData.ToLower().Contains(contentFilter.ToLower()))
            return false;
            
        return true;
    }
    
    /// <summary>
    /// Handler for when the type filter changes
    /// </summary>
    private void OnFilterTypeChanged(int index)
    {
        if (index == 0) // "All" option
        {
            typeFilter = null;
        }
        else
        {
            typeFilter = (SimLogType)(index - 1); // -1 because "All" is at index 0
        }
        
        RefreshLogs();
    }
    
    /// <summary>
    /// Handler for when the content filter changes
    /// </summary>
    private void OnFilterContentChanged(string value)
    {
        contentFilter = value;
        RefreshLogs();
    }
    
    /// <summary>
    /// Clears all logs
    /// </summary>
    public void ClearLogs()
    {
        LoggingService.Instance.ClearLogs();
    }
    
    /// <summary>
    /// Toggles auto-scrolling
    /// </summary>
    public void ToggleAutoScroll(bool value)
    {
        autoScroll = value;
    }
}
