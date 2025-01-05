using PixelCrushers.DialogueSystem;
using UnityEngine;
using DG.Tweening;
using PixelCrushers;
using System.Collections.Generic;
using System;

public class DialoguePlayManager : MonoBehaviour
{
    [Serializable]
    public class DialogueTypeReference
    {
        public ProgressionType ProgressionType;
        public DialogueSystemTrigger DialogueSystemTrigger;
    }

    [Serializable]
    public class DialogueReference
    {
        public int Day;
        public List<DialogueTypeReference> DialogueTypeReferences = new List<DialogueTypeReference>();
    }

    [SerializeField] private float dialogueStartDelay = 1f;
    [SerializeField] private List<DialogueReference> dialogueReferences = new List<DialogueReference>();

    private DialogueSystemTrigger dialogueSystemTrigger;

    private void Start()
    {
        int currentDay = GameDataManager.Instance.CurrentDay;
        if (!TryGetDialogueSystemTrigger(currentDay, out dialogueSystemTrigger))
        {
            Debug.LogError($"Dialogue trigger not found for Day {currentDay}. Check your dialogue references or progression type.");
            return;
        }

        // Delay and start dialogue
        Invoke(nameof(StartDialogue), dialogueStartDelay);

        // Remove the message to avoid reuse
        CrossSceneMessage.Remove(currentDay.ToString());
    }

    private void StartDialogue()
    {
        if (dialogueSystemTrigger != null)
        {
            dialogueSystemTrigger.OnUse();
        }
        else
        {
            Debug.LogError("DialogueSystemTrigger is null. Dialogue cannot be started.");
        }
    }

    private bool TryGetDialogueSystemTrigger(int currentDay, out DialogueSystemTrigger trigger)
    {
        trigger = null;

        // Find dialogue reference for the current day
        DialogueReference dialogueReference = dialogueReferences.Find(i => i.Day == currentDay);
        if (dialogueReference == null)
        {
            Debug.LogWarning($"No dialogue reference found for Day {currentDay}.");
            return false;
        }

        // Get the current progression type
        ProgressionType progressionType = CrossSceneMessage.GetProgressionType(currentDay.ToString());

        // Find the specific dialogue system trigger for the progression type
        DialogueTypeReference typeReference = dialogueReference.DialogueTypeReferences
            .Find(i => i.ProgressionType == progressionType);

        if (typeReference == null || typeReference.DialogueSystemTrigger == null)
        {
            Debug.LogWarning($"No dialogue trigger found for ProgressionType '{progressionType}' on Day {currentDay}.");
            return false;
        }

        trigger = typeReference.DialogueSystemTrigger;
        return true;
    }
}
