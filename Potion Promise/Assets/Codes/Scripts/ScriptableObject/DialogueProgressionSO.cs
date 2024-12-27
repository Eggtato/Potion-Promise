using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueProgressionSO", menuName = "Eggtato/Dialogue Progression")]
public class DialogueProgressionSO : ScriptableObject
{
    public List<DialogueProgressionData> DialogueProgressionDataList = new List<DialogueProgressionData>();
}
