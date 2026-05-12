using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public static event Action<DialogueSO> OnDialogueOptionSelected;
    [Header("UI References")]
    public Image portrait;
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    public bool isDialogueActive;
    public CanvasGroup canvasGroup;
    public Button[] choiceButtons;


    public float lastDialogueEndTime;
    private float dialogueCooldown = 0.1f;

    private DialogueSO currentDialogue;
    private int dialogueIndex;



    void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public bool canStartDialogue()
    {
        return Time.unscaledTime - lastDialogueEndTime >= dialogueCooldown;
    }


    public void StartDialogue(DialogueSO dialogue)
    {
        ClearChoices();
        currentDialogue = dialogue;
        dialogueIndex = 0;
        isDialogueActive = true;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        ShowDialogue();
    }

    public void AdvanceDialogue()
    {
        ClearChoices();
        if (dialogueIndex < currentDialogue.lines.Length)
        {
            ShowDialogue();
        }
        else
        {
            ShowOptions();
        }
    }

    private void ShowDialogue()
    {
        DialogueLine line = currentDialogue.lines[dialogueIndex];

        GameManager.instance.dialogueHistoryTracker.RecordNPC(line.speaker);

        portrait.sprite = line.speaker.portrait;
        actorName.text = line.speaker.actorName;
        dialogueText.text = line.text;

        dialogueIndex++;
    }

    private void ShowOptions()
    {
        if (currentDialogue.options.Length > 0)
        {
            for (int i = 0; i < currentDialogue.options.Length; i++)
            {
                var option = currentDialogue.options[i];

                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = option.optionText;
                choiceButtons[i].gameObject.SetActive(true);

                choiceButtons[i].onClick.AddListener(() => ChooseOption(option.nextDialogue));
            }
        }
        else if (currentDialogue.offerQuestOnEnd != null)
        {
            QuestEvents.onQuestOfferRequested?.Invoke(currentDialogue.offerQuestOnEnd);
            EndDialogue();
        }
        else
        {
            choiceButtons[0].GetComponentInChildren<TMP_Text>().text = "Exit";
            choiceButtons[0].onClick.AddListener(EndDialogue);
            choiceButtons[0].gameObject.SetActive(true);
            // EventSystem.current.SetSelectedGameObject(choiceButtons[0].gameObject);
        }
    }


    private void ChooseOption(DialogueSO dialogueSO)
    {
        if (dialogueSO == null)
        {
            EndDialogue();
        }
        else
        {
            ClearChoices();
            StartDialogue(dialogueSO);
            OnDialogueOptionSelected?.Invoke(dialogueSO);
        }
    }

    private void EndDialogue()
    {
        if(currentDialogue != null)
        {
            EventBus.Publish(
                currentDialogue.dialogueId,
                new DialogueFinishedEvent(string.Empty, currentDialogue.dialogueId)
            );
        }
        dialogueIndex = 0;
        isDialogueActive = false;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        lastDialogueEndTime = Time.unscaledTime;
    }

    private void ClearChoices()
    {
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
    }
}
