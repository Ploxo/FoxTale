using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SensorController : MonoBehaviour
{
    public enum SensorType
    {
        ACCELEROMETER,
        STEPCOUNTER
    }

    public static SensorController Instance;

    private Accelerometer accelerometer;
    private StepCounter stepCounter;

    [SerializeField]
    private TextMeshProUGUI accelerometerText;
    [SerializeField]
    private TextMeshProUGUI counterText;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        accelerometer = Accelerometer.current;
        if (accelerometer != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
            Debug.Log("Accelerometer is enabled");
        }
        else
        {
            Debug.Log("Accelerometer is null");
        }

        stepCounter = StepCounter.current;
        if (stepCounter != null)
        {
            InputSystem.EnableDevice(StepCounter.current);
            Debug.Log("StepCounter is enabled");
        }
        else
        {
            Debug.Log("StepCounter is null");
        }
    }

    private void Update()
    {
        if (accelerometer != null)
        {
            accelerometerText.text = "Accelerometer: " + accelerometer.acceleration.ReadValue();

            Vector3 position = new Vector3(-1, 0, 0);
            //Debug.DrawLine(position, position + accelerometer.acceleration.ReadValue());
            Debug.DrawLine(position, position + Vector3.up, Color.red);
        }
        if (stepCounter != null)
        {
            counterText.text = "StepCounter: " + stepCounter.stepCounter.ReadValue();
        }
    }

    public int CurrentStepsTaken()
    {
        //if (stepCounter == null)
        //    return stepsTaken;

        return stepCounter.stepCounter.ReadValue();
    }

    public Vector3 CurrentAcceleration()
    {
        return accelerometer.acceleration.ReadValue();
    }

    public int stepsTaken = 0;
    public void CountUp()
    {
        stepsTaken++;
    }

}
