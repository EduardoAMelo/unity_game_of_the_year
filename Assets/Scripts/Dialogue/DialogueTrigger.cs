using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    private bool playerInRange;
    private UnityEngine.InputSystem.PlayerInput playerInput;
    private void Awake()
    {
        visualCue.SetActive(false);
        playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialoguePlaying)
        {
            visualCue.SetActive(true);

            bool interactPressed = false;

            if (playerInput == null)
                playerInput = FindObjectOfType<UnityEngine.InputSystem.PlayerInput>();

            if (playerInput != null)
            {
                var interact = playerInput.actions.FindAction("Interact");
                if (interact != null && interact.triggered)
                    interactPressed = true;

                var open = playerInput.actions.FindAction("Open");
                if (!interactPressed && open != null && open.triggered)
                    interactPressed = true;

                var submit = playerInput.actions.FindAction("Submit");
                if (!interactPressed && submit != null && submit.triggered)
                    interactPressed = true;

                var fire = playerInput.actions.FindAction("Fire");
                if (!interactPressed && fire != null && fire.triggered)
                    interactPressed = true;
            }

            if (!interactPressed && Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame)
                interactPressed = true;

            if (interactPressed)
            {
                Debug.Log("Starting dialogue from trigger.");
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            visualCue.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            visualCue.SetActive(false);
            playerInRange = false;
        }
    }
}