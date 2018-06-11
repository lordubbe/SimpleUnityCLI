using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFYG.CLI;

[CommandParameterParser(typeof(string))]
public class StringParser : CLIParameterParser<string>
{
    public override string Parse(string valueToParse)
    {
        if (valueToParse.StartsWith("\"") && valueToParse.EndsWith("\""))
        {
            string stringValue = valueToParse.Substring(1, valueToParse.Length - 2); // Get everything but the quotation marks

            return stringValue;
        }

        CLI.Print(this + ": Expected string parameter to be inside quotation marks!");
        return null;
    }
}
