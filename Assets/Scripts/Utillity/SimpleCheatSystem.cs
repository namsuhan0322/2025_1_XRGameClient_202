using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SimpleCheatSystem : MonoBehaviour
{
    public static SimpleCheatSystem instance { get; private set; }

    [Header("UI ���۷���")]
    [SerializeField] private GameObject cheatPanel;
    [SerializeField] private TMP_InputField commandInput;
    [SerializeField] private TextMeshProUGUI outputText;

    [Header("����")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F1;

    private Dictionary<string, System.Action<string[]>> commands;
    private List<string> outputLines = new List<string>();
    private bool isActive = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeCommands();
        }
        else
        {
            Destroy(gameObject);
        }            
    }

    private void Start()
    {
        if (cheatPanel != null)
        {
            cheatPanel.SetActive(false);
        }
        Log("ġƮ �ý��� �غ� �Ϸ�. F1 Ű�� ����");
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            TogglePanel();
        }
        if (isActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            ExecuteCommand();
        }
    }

    private void InitializeCommands()
    {
        commands = new Dictionary<string, System.Action<string[]>>
        {
            { "god", ToggleGodMod},
            { "kill", KillPlayer},
            { "clear", ClearConsole},
            { "help", ShowHelp}
        };
    }

    private void ToggleGodMod(string[] args)
    {
        Log("���� ��� ��۵�");
        ShowToast("���� ��� ON/OFF", ToastMessage.MessageType.Success);

        // TODO: ���� ���� ��� ���� 
    }

    private void KillPlayer(string[] args)
    {
        Log("�÷��̾� ���");
        ShowToast("�÷��̾� ���!", ToastMessage.MessageType.Error);

        // TODO: �÷��̾� ��� ó�� ����
    }

    private void ClearConsole(string[] args)
    {
        outputLines.Clear();
        if (outputText != null)
            outputText.text = "";
        Log("�ܼ� ������");
    }

    private void ShowHelp(string[] args)
    {
        Log("=== ġƮ ��ɾ� ===");
        Log("god - �������");
        Log("Kill - �÷��̾� ���̱�");
        Log("clear - �ܼ� ����");
    }

    private void Log(string message, bool isError = false)
    {
        string coloredMessage = isError ? $"<color=red>{message}</color>" : message;
        outputLines.Add(coloredMessage);

        if (outputLines.Count > 10)
            outputLines.RemoveAt(0);

        if (outputText != null)
        {
            outputText.text = string.Join("\n", outputLines);
        }
    }

    private void ShowToast(string message, ToastMessage.MessageType type = ToastMessage.MessageType.info)
    {
        if (ToastMessage.instance != null)
        {
            ToastMessage.instance.ShowMessage($"[ġƮ] {message}", type);
        }
    }

    private void ExecuteCommand()
    {
        if (commandInput == null || string.IsNullOrEmpty(commandInput.text))
            return;

        string command = commandInput.text.Trim().ToLower();
        string[] parts = command.Split(' ');

        Log($"> {command}");

        if (commands.ContainsKey(parts[0]))
        {
            commands[parts[0]](parts);
        }
        else
        {
            Log($"�˼� ���� ��ɾ� : {parts[0]}", true);
        }

        commandInput.text = "";
        commandInput.ActivateInputField();
    }

    private void TogglePanel()
    {
        isActive = !isActive;

        if (cheatPanel != null)
        {
            cheatPanel.SetActive(isActive);

            if (isActive && commandInput != null)
            {
                commandInput.Select();
                commandInput.ActivateInputField();
            }
        }
    }
}
