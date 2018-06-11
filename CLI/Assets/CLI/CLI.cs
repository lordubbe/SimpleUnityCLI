using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFYG.CLI
{
    public enum MessageType
    {
        Log,
        Warning,
        Error
    };

    public static class CLI
    {
        public static void Print(string message, MessageType messageType = MessageType.Log, float displayTime = CLIPreferences.LOG_DEFAULT_DISPLAY_TIME)
        {
            CLIController.Instance.LogToConsole(message, messageType, displayTime);
        }
    }
}
