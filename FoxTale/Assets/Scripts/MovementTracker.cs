using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovementTracker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI accelerometerText;

    private Vector3 accelerometerValue;


    void Start()
    {
        Input.compensateSensors = false;
    }

    void Update()
    {
        accelerometerValue = Input.acceleration;

        accelerometerText.text = accelerometerValue.ToString();
    }
}
