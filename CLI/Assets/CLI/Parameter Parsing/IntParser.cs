using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFYG.CLI;

[CommandParameterParser(typeof(int))]
public class IntParser : CLIParameterParser<int>
{
    public override int Parse(string valueToParse)
    {
        int result;
        if (int.TryParse(valueToParse, out result))
        {
            return result;
        }

        CLI.Print(this + ": Cannot parse required parameter as int. Returning " + default(int) + ".");
        return default(int);
    }
}
