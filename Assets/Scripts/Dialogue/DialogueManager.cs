using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    [Header("Debug")]
    [Tooltip("Optional: assign a TextAsset to open via the 'I' key for testing")] 
    [SerializeField] private TextAsset debugInk;

    private Story currentStory;
    private static DialogueManager instance;
    public bool dialoguePlaying { get; private set;}
    private UnityEngine.InputSystem.PlayerInput playerInput;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of DialogueManager found!");
        }
        instance = this;
    }
    
    public static DialogueManager GetInstance()
    {
        return instance;
    }
    
    private void Start()
    {
        dialoguePlaying = false;
        dialoguePanel.SetActive(false);
        playerInput = FindObjectOfType<UnityEngine.InputSystem.PlayerInput>();
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialoguePlaying = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void Update()
    {
        // If not in dialogue, listen for the 'I' key (or an "Open" action if available) to open a test dialogue
        if (!dialoguePlaying)
        {
            bool opened = false;
            if (playerInput != null)
            {
                var openAction = playerInput.actions.FindAction("Open");
                if (openAction != null && openAction.triggered)
                {
                    if (debugInk != null)
                        EnterDialogueMode(debugInk);
                    opened = true;
                }
            }

            if (!opened && Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
            {
                if (debugInk != null)
                    EnterDialogueMode(debugInk);
            }

            return;
        }

        // When in dialogue, advance on Space or on the configured PlayerInput action (Submit or Fire)
        bool continuePressed = false;

        if (playerInput != null)
        {
            var submit = playerInput.actions.FindAction("Submit");
            if (submit != null && submit.triggered)
                continuePressed = true;
            else
            {
                var fire = playerInput.actions.FindAction("Fire");
                if (fire != null && fire.triggered)
                    continuePressed = true;
            }
        }

        if (!continuePressed && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            continuePressed = true;

        if (continuePressed)
            ContinueStory();
    }
    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialoguePlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        currentStory = null;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }
}