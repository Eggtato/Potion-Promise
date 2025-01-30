using UnityEngine;
using MoreMountains.Feedbacks;
using Febucci.UI;

public class DialogueTypeSound : MonoBehaviour
{
    [Tooltip("How much time has to pass before playing the next sound")]
    [SerializeField] private float minSoundDelay = .07f;

    private TypewriterByCharacter typewriterByCharacter;
    private float latestTimePlayed = -1;

    private void Awake()
    {
        typewriterByCharacter = GetComponent<TypewriterByCharacter>();
    }

    private void OnEnable()
    {
        typewriterByCharacter.onCharacterVisible.AddListener(OnCharacterVisible);
    }

    private void OnDisable()
    {
        typewriterByCharacter.onCharacterVisible.RemoveListener(OnCharacterVisible);
    }

    private void OnCharacterVisible(char letter)
    {
        if (Time.time - latestTimePlayed <= minSoundDelay)
            return; //Early return if not enough time passed yet

        AudioManager.Instance.PlayTypeSound();

        latestTimePlayed = Time.time;
    }
}
