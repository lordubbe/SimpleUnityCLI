using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFYG.CLI;

public class TestScript : MonoBehaviour
{
    [ConsoleCommand]
    public static void HelloWorld()
    {
        CLI.Print("Hello World!");
    }

    [ConsoleCommand]
    public static void TestWarning()
    {
        CLI.Print("This is a warning!", MessageType.Warning);
    }

    [ConsoleCommand]
    public static void TestError()
    {
        CLI.Print("This is an error!", MessageType.Error);
    }

    [ConsoleCommand]
    public static void LogMessage(string message, MessageType messageType = MessageType.Log)
    {
        CLI.Print(message, messageType);
    }

    [ConsoleCommand]
    public void ChangeColor(Color color)
    {

    }
}
