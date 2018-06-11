using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MFYG.CLI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandParameterParser : PropertyAttribute
    {
        public CommandParameterParser(Type parsedType)
        {

        }
    }

    public abstract class CLIParameterParser<T>
    {
        public abstract T Parse(string valueToParse);
    }
}