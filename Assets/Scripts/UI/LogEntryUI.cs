using UnityEngine;
using TMPro;

/// <summary>
/// UI component for a single log entry
/// </summary>
public class LogEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private RectTransform backgroundRect;
    
    private LogEntry logEntry;
    
    /// <summary>
    /// Initializes the log entry UI with data
    /// </summary>
    public void Initialize(LogEntry entry)
    {
        logEntry = entry;
        
        if (logText != null)
        {
            logText.text = entry.FormattedMessage;
            logText.color = entry.GetColor();
        }
    }
    
    /// <summary>
    /// Displays additional information when clicked
    /// </summary>
    public void OnClick()
    {
        // Toggle expanded view with context data if available
        if (!string.IsNullOrEmpty(logEntry.ContextData))
        {
            bool isExpanded = logText.text.Contains(logEntry.ContextData);
            
            if (isExpanded)
            {
                // Collapse
                logText.text = logEntry.FormattedMessage;
            }
            else
            {
                // Expand
                logText.text = $"{logEntry.FormattedMessage}\n<size=80%>{logEntry.ContextData}</size>";
            }
            
            // Adjust the height
            Canvas.ForceUpdateCanvases();
            if (backgroundRect != null)
            {
                backgroundRect.sizeDelta = new Vector2(
                    backgroundRect.sizeDelta.x,
                    logText.preferredHeight + 10 // 5px padding on top and bottom
                );
            }
        }
    }
}
