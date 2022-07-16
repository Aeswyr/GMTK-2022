using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class CommandPromptWindow : EditorWindow
{
    [MenuItem("Window/CommandPromptWindow _'")]
    public static void ShowCommandPrompt()
    {
        // Get existing open window or if none, make a new one:
        CommandPromptWindow prompt = (CommandPromptWindow)EditorWindow.GetWindow(typeof(CommandPromptWindow));
        prompt.minSize = new Vector2(300, 20);
        prompt.maxSize = new Vector2(300, 20);
        var mPos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        prompt.position = new Rect(mPos.x, mPos.y, 300, 20);
        firstFocus = true;
        prompt.Show();
        WindowInst = prompt;

        //GUI.FocusWindow()
        GUI.FocusControl("PromptField");
    }

    private static bool firstFocus = true;
    private string inputBuffer;
    private int inputCursor;

    private static CommandPromptWindow WindowInst;

    public static string[] CommandStrings = new string[0]; // set in RefreshDebugCommands

    private struct SuggestionElement
    {
        public string Text;
        public bool Visible;

        public void SetVisible(bool v)
        {
            Visible = v;
            if (!v) Text = "";
        }

        public void SetText(string text)
        {
            Text = text;
        }

        public string GetText()
        {
            return Text;
        }
    }

    private SuggestionElement[] suggestionElements = new SuggestionElement[5];

    private void OnGUI()
    {
        // refresh available commands
        if (CommandMapping.Count <= 0) RefreshDebugCommands();

        // create an input field and focus it
        GUI.SetNextControlName("PromptField");
        string newInput = GUI.TextField(new Rect(1f, 2.5f, 250, 15), inputBuffer);
        if (string.CompareOrdinal(newInput, inputBuffer) != 0)
        {
            OnValueChanged(newInput); // sets inputBuffer to newInput
        }
        if (firstFocus)
        {
            GUI.FocusControl("PromptField");
            firstFocus = false;
        }

        // show suggestions if available
        int visibleCount = 0;
        for (int i = 0; i < suggestionElements.Length; i++)
        {
            if (suggestionElements[i].Visible) visibleCount++;
            GUI.Label(new Rect(1f, 12 * i + 16f, 250, 15), suggestionElements[i].GetText(), i == suggestionCursor ? EditorStyles.boldLabel : EditorStyles.label);
        }

        // window size changes based on number of suggestions
        this.minSize = new Vector2(300, 20 + 12 * visibleCount);
        this.maxSize = new Vector2(300, 20 + 12 * visibleCount);
    }

    [InitializeOnLoadMethod]
    private static void EditorInit()
    {
        InitEditorKeypressListener();
    }

    // get debug commands with reflection
    private void RefreshDebugCommands()
    {
        // map suggestions
        CommandMapping.Clear();
        MapCommands(typeof(DebugCommands));

        // create string list
        var keys = CommandMapping.Keys;
        CommandStrings = new string[keys.Count];
        //Debug.Log(CommandStrings.Length);
        keys.CopyTo(CommandStrings, 0);
        //Debug.Log(CommandStrings[0]);
    }

    private void MapCommands(Type t)
    {
        var commands = t.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
        for (int i = 0; i < commands.Length; i++)
        {
            var attributes = commands[i].GetCustomAttributes(false);
            for (int j = 0; j < attributes.Length; j++)
            {
                if (attributes[j].GetType() == typeof(CommandAttribute) || attributes[j].GetType() == typeof(MenuItem))
                {
                    System.Reflection.MethodInfo command = commands[i];
                    CommandMapping.Add(commands[i].Name, () => { command.Invoke(null, null); });
                    break;
                }
            }
        }
    }

    // reflection hack for getting access to keyboard events
    private static void InitEditorKeypressListener()
    {
        System.Reflection.FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        EditorApplication.CallbackFunction value = (EditorApplication.CallbackFunction)info.GetValue(null);
        value += EditorGlobalKeyPress;
        info.SetValue(null, value);
    }

    static void EditorGlobalKeyPress()
    {
        if (WindowInst == null) return;
        //Debug.Log("KEY CHANGE " + Event.current.keyCode);

        if (Event.current.keyCode == KeyCode.DownArrow)
        {
            WindowInst.MoveSuggestionCursor(1);
            WindowInst.Repaint();
        }
        else if (Event.current.keyCode == KeyCode.UpArrow)
        {
            WindowInst.MoveSuggestionCursor(-1);
            WindowInst.Repaint();
        }

        else if (Event.current.keyCode == KeyCode.Return)
        {
            if (WindowInst.suggestionCursor < 0)
                WindowInst.UseFirstSuggestion(WindowInst.inputBuffer);
            WindowInst.Parse(WindowInst.inputBuffer);
            WindowInst.Close();
        }
        else if (Event.current.keyCode == KeyCode.Escape)
        {
            WindowInst.Close();
        }
    }


    public static readonly Dictionary<string, Action> CommandMapping = new Dictionary<string, Action>();

    private int suggestionCursor = -1;
    private string unsuggestedInput;
    private int changeIgnoreCounter;

    private readonly List<int> selectedShips = new List<int>();

    private void MoveSuggestionCursor(int amount)
    {
        //Debug.Log("move selection cursor: " + amount);

        // early out for invalid "down"
        if (amount < 0 && suggestionCursor < 0) return;

        // special case, pressed down to go back to original
        if (amount < 0 && suggestionCursor == 0)
        {
            //Debug.Log("go to original: " + unsuggestedInput);
            inputBuffer = unsuggestedInput;
            ClearSelectionCursor();
            UpdateSelectionUI();
            return;
        }

        // move cursor
        bool isPlayersTextSaved = !string.IsNullOrEmpty(unsuggestedInput);
        if (!isPlayersTextSaved)
        {
            unsuggestedInput = inputBuffer;
        }
        var suggestions = GetSuggestions(unsuggestedInput);


        // don't do anything if there are no suggestions
        if (suggestions.Count <= 0)
        {
            ClearSelectionCursor();
            UpdateSelectionUI();
            return;
        }

        // move selection
        suggestionCursor += amount;
        suggestionCursor = Mathf.Min(suggestions.Count - 1, suggestionCursor); // cannot go above top
        changeIgnoreCounter++;
        inputBuffer = suggestions[Mathf.Max(0, suggestionCursor)];
        UpdateSelectionUI();
        MoveToEnd();
    }

    private void MoveToEnd()
    {
         // can't
    }

    private void UpdateSelectionUI()
    {
        //// update selection ui
        //for (int i = 0; i < suggestionElements.Length; i++)
        //{
        //    suggestionElements[i].Background.color = i == suggestionCursor
        //        ? new Color(0.8f, 0.8f, 0.8f, 0.1f)
        //        : new Color(0, 0, 0, 0.1f);
        //}
    }

    private void ClearSelectionCursor()
    {
        unsuggestedInput = null;
        suggestionCursor = -1;
    }

    private void OnValueChanged(string inputText)
    {
        inputBuffer = inputText;
        //Debug.Log("value changed " + inputText);
        if (changeIgnoreCounter > 0)
        {
            //Debug.Log("change ignored " + inputText);
            changeIgnoreCounter--;
            return;
        }
        ClearSelectionCursor();
        HandleSpecial(inputText);
        SuggestOptions(inputText);
    }

    private void OnSubmit(string submission)
    {
        Parse(submission);
        inputBuffer = "";
        FocusInput();

    }

    private void FocusInput()
    {
        //inputField.Select();
        //inputField.ActivateInputField();
    }

    private void Parse(string input)
    {
        //Debug.Log("Parse " + input);
        List<string> candidates = new List<string>();
        foreach (var command in CommandStrings)
        {
            if (input.ToLower() == command.ToLower())
            {
                //Debug.Log("Handle " + input);
                Debug.Log("Handle command " + command);
                HandleCommand(CommandMapping[command], input.Substring(command.Length));
                return;
            } 
            else if (input.IndexOf(command, StringComparison.OrdinalIgnoreCase) == 0)
            {
                candidates.Add(command);
            }
        }

        int maxLength = 0;
        string best = "";
        foreach (var command in candidates)
        {
            if (maxLength < command.Length) { 
                best = command; 
                maxLength = best.Length; 
            }
        }
        if (maxLength > 0)
        {
            Debug.Log("Handle command " + best);
            HandleCommand(CommandMapping[best], input.Substring(best.Length));
            return;
        }
    }

    private void HandleCommand(Action command, string args)
    {
        var argsSplit = args.Split(' ');
        command?.Invoke();
    }

    private void HandleSpecial(string fullValue)
    {
        if (fullValue.Length <= 0) return;
        char lastChar = fullValue[fullValue.Length - 1];
        switch (lastChar)
        {
            case '\t':
                Debug.Log("Handle Tab");
                MoveSuggestionCursor(1);
                break;
        }
    }

    private void UseFirstSuggestion(string input)
    {
        var suggestions = GetSuggestions(input);
        if (suggestions.Count <= 0) return;
        changeIgnoreCounter++;
        inputBuffer = suggestions[0];
        MoveToEnd();
    }

    private void SuggestOptions(string input)
    {
        List<string> suggestions = GetSuggestions(input);
        for (int i = 0; i < suggestions.Count && i < suggestionElements.Length; i++)
        {
            suggestionElements[i].SetVisible(true);
            suggestionElements[i].SetText(suggestions[i]);
        }
        for (int i = suggestions.Count; i < suggestionElements.Length; i++)
        {
            suggestionElements[i].SetVisible(false);
        }
    }

    private static List<string> GetSuggestions(string source)
    {
        List<string> suggestions = new List<string>();
        if (source.Length <= 0) return suggestions;

        string sourceLower = source.ToLower();

        // create list of commands and remove from it when a command is used
        List<string> unsuggested = new List<string>();

        // fill initial suggestions
        foreach (var command in CommandStrings)
        {
            if (command.Length >= sourceLower.Length)
            {
                int sourceIndex = 0;
                int commandIndex = 0;
                bool found = true;
                for (sourceIndex = 0; sourceIndex < sourceLower.Length; sourceIndex++)
                {
                    char c = sourceLower[sourceIndex];
                    for (; commandIndex <= command.Length; commandIndex++)
                    {
                        if (commandIndex >= command.Length)
                        {
                            found = false;
                            break;
                        }
                        if (char.ToLower(command[commandIndex]) == char.ToLower(c))
                        {
                            commandIndex++;
                            break;
                        }
                    }
                    if (!found) break;
                }
                if (found) suggestions.Add(command);
            }
        }

        //string debug = "";
        //debug = "";
        //foreach (var s in suggestions) debug += s + ",";
        //Debug.Log("pre-sort: " + debug);

        // sort suggestions
        suggestions.Sort((a, b) =>
        {
            // case-insensitive
            string aLower = a.ToLower();
            string bLower = b.ToLower();

            // sorted first by first appearance of adjacent sequence
            if (source.Length > 1)
            {
                int aAdjSeqIndex = a.IndexOf(source, StringComparison.OrdinalIgnoreCase);
                int bAdjSeqIndex = b.IndexOf(source, StringComparison.OrdinalIgnoreCase);
                // both words contrain the full sequence
                if (aAdjSeqIndex >= 0 && bAdjSeqIndex >= 0)
                {
                    // not a tie
                    if (aAdjSeqIndex != bAdjSeqIndex)
                        return aAdjSeqIndex - bAdjSeqIndex;
                    
                }
                // only one word contains the full sequence
                else if (aAdjSeqIndex >= 0 || bAdjSeqIndex >= 0)
                {
                    if (bAdjSeqIndex >= 0) return 1;
                    else return -1;
                }
                // neither word contains the full sequence
                // treat the characters as separate for tiebreaker
            }

            // sorted by first appearance of first character,
            // ties are broken by next character, and so on
            int cursorA = 0;
            int cursorB = 0;
            for (int i = 0; i < sourceLower.Length; i++)
            {
                int indexA = aLower.IndexOf(sourceLower[i], cursorA);
                int indexB = bLower.IndexOf(sourceLower[i], cursorB);

                // advance cursors, never go backwards
                if (indexA > cursorA) cursorA = indexA;
                if (indexB > cursorB) cursorB = indexB;

                //Debug.Log($"index of '{sourceLower[i]}'  '{a}': {cursorA}, '{b}': {cursorB}");

                // tie, advance to next character
                if (indexA == indexB) continue;

                // not tied, one is the winner
                //Debug.Log($"sort '{a}' === ({indexA - indexB}) === '{b}'");
                return indexA - indexB; // winner is whichever is first
            }
            //Debug.Log($"tie for '{a}' vs '{b}'");
            return string.CompareOrdinal(a,b);
        });

        //debug = "";
        //foreach (var s in suggestions) debug += s + ",";
        //Debug.Log("post-sort: " + debug);

        return suggestions;
    }
}
