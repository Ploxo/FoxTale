using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SensorController : MonoBehaviour
{
    public DrawLine drawLine;

    public enum SensorType
    {
        ACCELEROMETER,
        STEPCOUNTER
    }

    public static SensorController Instance;

    private Accelerometer accelerometer;
    private StepCounter stepCounter;

    private LinearAccelerationSensor linearSensor;
    [SerializeField]
    private TextMeshProUGUI linearText;
    private GravitySensor gravitySensor;
    [SerializeField]
    private TextMeshProUGUI gravityText;

    [SerializeField]
    private TextMeshProUGUI accelerometerText;
    [SerializeField]
    private TextMeshProUGUI counterText;

    private float accelerometerUpdateInterval = 1 / 60f;
    private float lowPassKernelWidthInSeconds = 1f;
    private float lowPassFilterFactor;
    private Vector3 lowPassValue;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        //shakeDetectionThreshold *= shakeDetectionThreshold; // squared for avoiding magnitude
        lowPassValue = CurrentLinearAcceleration();

        Initialize();
    }

    private void Initialize()
    {
        accelerometer = Accelerometer.current;
        if (accelerometer != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
            Accelerometer.current.samplingFrequency = accelerometerUpdateInterval;
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

        linearSensor = LinearAccelerationSensor.current;
        if (linearSensor != null)
        {
            InputSystem.EnableDevice(LinearAccelerationSensor.current);
            Debug.Log("LinearAccelerationSensor is enabled");
        }
        else
        {
            Debug.Log("LinearAccelerationSensor is null");
        }

        gravitySensor = GravitySensor.current;
        if (gravitySensor != null)
        {
            InputSystem.EnableDevice(GravitySensor.current);
            Debug.Log("GravitySensor is enabled");
        }
        else
        {
            Debug.Log("GravitySensor is null");
        }
    }

    private void Update()
    {
        if (accelerometer != null)
        {
            accelerometerText.text = "Accelerometer: " + accelerometer.acceleration.ReadValue();

            //Vector3 position = new Vector3(0, 3, 0);
            //drawLine.positionOne = position;
            //drawLine.positionTwo = position + accelerometer.acceleration.ReadValue();
            //drawLine.positionTwo = position + Vector3.up;
            //drawLine.color1 = Color.red;
            //drawLine.color2 = Color.blue;
        }
        if (stepCounter != null)
        {
            //counterText.text = "StepCounter: " + stepCounter.stepCounter.ReadValue();
        }
        if (linearSensor != null)
        {
            //linearText.text = "LinearAccelerationSensor: " + linearSensor.acceleration.ReadValue();
        }
        if (gravitySensor != null)
        {
            //gravityText.text = "GravitySensor: " + gravitySensor.gravity.ReadValue();
        }
    }

    public int CurrentStepsTaken()
    {
        if (stepCounter == null)
            return stepsTaken;

        return stepCounter.stepCounter.ReadValue();
    }

    public Vector3 CurrentAcceleration()
    {
        return accelerometer.acceleration.ReadValue();
    }

    public Vector3 CurrentLinearAcceleration()
    {
        Vector3 acceleration = accelerometer.acceleration.ReadValue();
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        return deltaAcceleration;

        return linearSensor.acceleration.ReadValue();
    }

    public Vector3 CurrentGravity()
    {
        return gravitySensor.gravity.ReadValue();
    }

    public int stepsTaken = 0;
    public void CountUp()
    {
        stepsTaken++;
    }

}
