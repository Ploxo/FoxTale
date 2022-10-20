using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepTracker : MonoBehaviour
{
    SensorController sensorController;

    private int initialSteps;
    private int currentSteps;
    private float stepSize = 0.5f;

    public int StepsTaken => currentSteps;
    public float DistanceTravelled => stepSize * currentSteps;


    void Start()
    {
        sensorController = SensorController.Instance;

        initialSteps = sensorController.CurrentStepsTaken();
    }

    void Update()
    {
        currentSteps = sensorController.CurrentStepsTaken() - initialSteps;
    }
}
