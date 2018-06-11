using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Reflection;
using System.Linq;
using MFYG.CLI;

using Object = UnityEngine.Object;

namespace MFYG.CLI
{
    public class CLIController : Singleton<CLIController>
    {
        [Header("Object Links")]
        public InputField inputField;
        public Button enterButton;

        public LogEntry logEntryTemplate;
        public Transform logEntryParent;

        public AutoCompleteUI autoCompleteUI;


        #region private variables
        // Options variables
        private Dictionary<string, MethodInfo> methods;
        private Dictionary<string, PropertyInfo> properties;
        private Dictionary<string, FieldInfo> fields;

        // System variables
        private List<CommandPart> commandBuilder = new List<CommandPart>();
        private int alreadyCheckedUntilIdx = 0;
        //private Dictionary<Type, System.Func> syntaxOptionDictionary;

        // UI
        private bool unresolvedInput = false;
        private int currentCaretPosition;
        private List<LogEntry> logEntries = new List<LogEntry>();
        #endregion

        private void Awake()
        {
            InitializeCommands();
            InitializeOptionDictionary();
        }

        private void OnEnable()
        {
            enterButton.onClick.AddListener(SubmitCommand);

            inputField.onValueChanged.AddListener(CommandChanged);
            inputField.onEndEdit.AddListener(SubmitCommandViaEnter);
            inputField.Select();

            currentCaretPosition = inputField.caretPosition;
        }

        private void OnDisable()
        {
            enterButton.onClick.RemoveListener(SubmitCommand);
            inputField.onValueChanged.RemoveListener(CommandChanged);
        }

        public void CommandChanged(string newCommand)
        {
            StartCoroutine(WaitAndUpdateAutoComplete());

            unresolvedInput = inputField.text.Length > 0 && inputField.text[inputField.text.Length - 1] == ' '; //TODO: Very simple test. Should be changed depending on whether there are autocomplete options available
        }

        private IEnumerator WaitAndUpdateAutoComplete()
        {
            yield return null;
            autoCompleteUI.transform.position = inputField.textComponent.transform.TransformPoint((inputField as CustomInputField).GetLocalCaretPosition());

            if (unresolvedInput)
            {
                //TODO: Get autocomplete options
                autoCompleteUI.gameObject.SetActive(true);
            }
            else
            {
                autoCompleteUI.gameObject.SetActive(false);
            }

        }

        public void SubmitCommand()
        {
            //TEST: Print a random number between 0 and 100 to the console
            if (string.IsNullOrEmpty(inputField.text))
            {
                CLI.Print((UnityEngine.Random.value * 100).ToString(), (MessageType)Enum.GetValues(typeof(MessageType)).GetValue(UnityEngine.Random.Range(0, 3)), UnityEngine.Random.Range(0.5f, 5f));
                ResetInput();
                return;
            }

            CLICommand command = CLICommand.CreateCommand(methods[inputField.text]);

            string error;
            if (command.IsValid(out error))
            {
                command.Execute();
            }
            else
            {
                CLI.Print(error, MessageType.Error);
            }


            ResetInput();
        }

        public void SubmitCommandViaEnter(string input)
        {
            SubmitCommand();
        }

        private void ResetInput()
        {
            commandBuilder.Clear();
            commandBuilder.Add(new CommandPart(new CommandStartToken()));

            inputField.text = "";

            inputField.Select();
            inputField.ActivateInputField();
        }

        private void InitializeOptionDictionary()
        {
            //syntaxOptionDictionary = new Dictionary<Type, Dictionary<string, MemberInfo>>()
            //{
            //    { typeof(CommandStartToken), methods as Dictionary<string, MethodBase> as Dictionary<string, MemberInfo> }
            //};
        }

        private void InitializeCommands()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type[] types = assemblies.SelectMany(a => a.GetTypes()).ToArray();

            // Get all methods with the [ConsoleCommand] attribute
            MethodInfo[] consoleCommands = types.SelectMany(t =>
                                                            t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Default)
                                                            .Where(a => a.IsDefined(typeof(ConsoleCommand), true))).ToArray();


            Debug.Log(consoleCommands.Count() + " commands found.");

            methods = new Dictionary<string, MethodInfo>();

            // Convert into selection options for the autocomplete system
            for (int i = 0; i < consoleCommands.Length; i++)
            {
                MethodInfo currentCommand = consoleCommands[i];

                string optionName = currentCommand.Name.SplitCamelCase().ToLower().Replace(' ', '-');
                methods.Add(optionName, currentCommand);
                Debug.Log(optionName + " added to 'methods'");
            }

            //TODO: Properties
            properties = new Dictionary<string, PropertyInfo>();

            //TODO: Fields
            fields = new Dictionary<string, FieldInfo>();
        }

        private void ParseInput()
        {
            string newInput = inputField.text.Substring(alreadyCheckedUntilIdx);
            alreadyCheckedUntilIdx = inputField.text.Length - 1;

            string[] arguments = newInput.Trim().Split(' ');

            if (arguments != null && arguments.Length > 0)
            {
                int argumentCount = arguments.Length;

                for (int i = 0; i < argumentCount; i++)
                {
                    string arg = arguments[i];

                    bool isMethod = methods.ContainsKey(arg);
                    bool isProperty = properties.ContainsKey(arg);
                    bool isField = fields.ContainsKey(arg);

                    if (isMethod)
                    {
                        MethodToken nextToken = new MethodToken();

                        if (commandBuilder.Last().Token.Match(nextToken))
                        {
                            //commandBuilder.Add(
                        }
                    }

                    if (isProperty)
                    {

                    }

                    if (isField)
                    {

                    }
                }
            }
        }

        //private List<CommandPart> GetAutoCompleteOptions()
        //{
        //    List<CommandPart> results = new List<CommandPart>();
        //    CommandPart latestPart = commandBuilder.Last();

        //    //methods.All(syntaxOptionDictionary[])
        //}

        public void LogToConsole(string message, MessageType messageType, float displayTime)
        {
            LogEntry logEntry = Instantiate(logEntryTemplate.gameObject, logEntryParent).GetComponent<LogEntry>();
            logEntry.SetMessage(message, messageType, displayTime);

            PostToConsole(logEntry);
        }

        private void PostToConsole(LogEntry logEntry)
        {
            logEntries.Add(logEntry);
            logEntry.gameObject.SetActive(true);
            logEntry.StartDisplay();
        }
    }

    public class CommandPart
    {
        public SyntaxToken Token;
        public string Text;
        public Object ObjectReference;

        public CommandPart(SyntaxToken token)
        {
            Token = token;
        }
    }
}