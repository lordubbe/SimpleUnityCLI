using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;

using Object = UnityEngine.Object;

namespace MFYG.CLI
{

    public abstract class CLICommand
    {
        public MemberInfo Command; 
        public List<Object> Parameters;
        public Object TargetObject;

        /// <summary>
        /// Execute the Command. Please only call this if you already validated the Command!
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Validates the current Command.
        /// </summary>
        /// <returns><c>true</c>, if the Command is valid, <c>false</c> otherwise.</returns>
        /// <param name="errorMessage">If the Command is invalid, this out parameter can be used for logging what went wrong.</param>
        public abstract bool IsValid(out string errorMessage);

        /// <summary>
        /// Creates the appropriate CLICommand type according to whether MethodInfo, PropertyInfo or FieldInfo was passed.
        /// </summary>
        /// <returns>The CLICommand which can be validated and then executed.</returns>
        /// <param name="memberInfo">Member info.</param>
        public static CLICommand CreateCommand(MemberInfo memberInfo)
        {
            MethodInfo memberAsMethod = memberInfo as MethodBase as MethodInfo;
            //TODO: Add Properties and Fields here...

            if (memberAsMethod != null)
            {
                return new CLIMethodCommand(memberAsMethod) { Command = memberAsMethod };
            }

            return null;
        }
    }

    public class CLIMethodCommand : CLICommand
    {
        private ParameterInfo[] parameterInfos;
        private int requiredParameters;

        public CLIMethodCommand(MethodInfo methodInfo)
        {
            parameterInfos = methodInfo.GetParameters();
            requiredParameters = parameterInfos.Count((p) => !p.IsOptional); // Might be double-confetti checking both for IsOptional and DefaultValue
            Parameters = new List<Object>();
        }

        public override void Execute()
        {
            MethodInfo method = Command as MethodBase as MethodInfo;
            method.Invoke(TargetObject, Parameters.ToArray());
        }

        public override bool IsValid(out string errorMessage)
        {
            MethodInfo methodInfo = Command as MethodBase as MethodInfo;

            if (methodInfo == null)
            {
                errorMessage = "ERROR: Unable to parse command (MemberInfo can't convert to MethodInfo)";
                return false;
            }

            if (!methodInfo.IsStatic && TargetObject == null)
            {
                errorMessage = "ERROR: Object reference required for non-static method! Please prefix the command with an object reference.";
                return false;
            }

            if (!methodInfo.IsStatic && TargetObject != null)
            {
                MethodInfo targetMethod = TargetObject.GetType().GetMethod(Command.Name);

                if (targetMethod == null)
                {
                    errorMessage = "ERROR: Method doesn't exist on object '" + TargetObject + "'!";
                    return false;
                }

                if (!targetMethod.GetCustomAttributes(true).Any((a) => (a as ConsoleCommand) != null))
                {
                    errorMessage = "ERROR: Method does not have the '" + typeof(ConsoleCommand).Name + "' attribute!";
                    return false;
                }
            }

            if (Parameters.Count < requiredParameters)
            {
                errorMessage = "ERROR: Expected " + requiredParameters + " parameters, but only got " + Parameters.Count + "!";
                return false;
            }

            if (Parameters.Count > parameterInfos.Length)
            {
                int paramDiff = parameterInfos.Length - requiredParameters;
                errorMessage = "ERROR: Too many parameters received! Expected " + parameterInfos.Length + (paramDiff != 0 ? "(" + (parameterInfos.Length - requiredParameters) + " optional)" : "") + " but got " + Parameters.Count;
                return false;
            }

            errorMessage = "";
            return true;
        }
    }

    public class CLIPropertyCommand : CLICommand
    {
        public override void Execute()
        {
            PropertyInfo property = Command as PropertyInfo;
            //property.SetValue(TargetObject, Parameters[0]. //TODO: Somehow try to convert to target type
        }

        public override bool IsValid(out string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}