using PixelCrushers.DialogueSystem;
using UnityEngine;
using DG.Tweening;
using PixelCrushers;

public class DialoguePlayManager : MonoBehaviour
{
    [SerializeField] private DialogueSystemTrigger dialogueStarter;
    [SerializeField] private float dialogueStartDelay = 1f;

    private void Start()
    {
        Invoke(nameof(StartDialogue), dialogueStartDelay);
    }

    private void StartDialogue()
    {
        dialogueStarter.OnUse();
    }
}
