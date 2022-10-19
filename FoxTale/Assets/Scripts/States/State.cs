using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public int stateId;

    public Sprite graphics;

    public Color backgroundColor;

    public Sentence[] sentences;

    public Option[] options;
}

[System.Serializable]
public class Sentence
{
    public Speaker speaker;
    [TextArea(10, 10)]
    public string text;
}

[System.Serializable]
public class Speaker
{
    public Sprite speakerSprite;
    public string speakerName;
    public Color speakerColor;
}