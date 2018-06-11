using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFYG.CLI;

[CommandParameterParser(typeof(float))]
public class FloatParser : CLIParameterParser<float>
{
    public override float Parse(string valueToParse)
    {
        float result;
        if (float.TryParse(valueToParse, out result))
        {
            return result;
        }

        CLI.Print(this + ": Cannot parse required parameter as float. Returning " + default(float) + ".");
        return default(float);
    }
}
