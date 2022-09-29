using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public int stateId;

    public Sprite graphics;

    [TextArea]
    public string text;

    public Option[] options;
}
