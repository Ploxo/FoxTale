using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogueDatabase", menuName = "Dialogue/Database")]
public class DialogueDatabase : ScriptableObject
{
    [SerializeField]
    private DialogueData[] dialogues;

    public DialogueData[] Dialogues
    {
        get
        {
            DialogueData[] newDialogues = new DialogueData[dialogues.Length];
            dialogues.CopyTo(newDialogues, 0);
            return newDialogues;
        }
    }
}
