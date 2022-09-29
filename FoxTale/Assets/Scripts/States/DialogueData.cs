using UnityEngine;

[System.Serializable]
public class DialogueData
{
    public Sprite background;
    [TextArea]
    public string[] dialogueSequence;
    public int[] options;
}