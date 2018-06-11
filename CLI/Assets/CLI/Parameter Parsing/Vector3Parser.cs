using System;
using UnityEngine;
using MFYG.CLI;
using System.Text.RegularExpressions;

[CommandParameterParser(typeof(Vector3))]
public class Vector3Parser : CLIParameterParser<Vector3>
{
    public override Vector3 Parse(string valueToParse)
    {
        if (valueToParse.StartsWith("(") && valueToParse.EndsWith(")"))
        {
            string vectorString = valueToParse.Substring(1, valueToParse.Length - 2); // Get everything but the parentheses
            vectorString = Regex.Replace(vectorString, @"\s+", ""); // Replace all whitespace with nothing

            string[] coeff = vectorString.Split(',');

            if (coeff.Length < 3)
            {
                CLI.Print(this + ": Expected 3 coefficients in the Vector3!", MessageType.Error);
            }
            else
            {
                return new Vector3(float.Parse(coeff[0]), float.Parse(coeff[1]), float.Parse(coeff[2]));
            }
        }

        CLI.Print(this + ": Expected Vector3 to start with '(' and end with ')'! Returning " + default(Vector3) + ".");
        return default(Vector3);
    }
}