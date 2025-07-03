using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DevConsoleUI : MonoBehaviour
{
    private ScrollView logScroll;
    private Label logText;

    private List<string> logLines = new();
    private const int maxLogLines = 200; // adjust to what feels smooth

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        logScroll = root.Q<ScrollView>("logScroll");
        logText = root.Q<Label>("logText");

        Application.logMessageReceived += HandleUnityLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleUnityLog;
    }

    void HandleUnityLog(string logString, string stackTrace, LogType type)
    {
        string formatted = type switch
        {
            LogType.Warning => $"<color=yellow>{logString}</color>",
            LogType.Error or LogType.Exception => $"<color=red>{logString}</color>",
            _ => logString
        };

        logLines.Add(formatted);

        if (logLines.Count > maxLogLines)
            logLines.RemoveAt(0);

        logText.text = string.Join("\n", logLines);
        logScroll.scrollOffset = new Vector2(0, float.MaxValue);
    }

}
