using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

public class SensorController : MonoBehaviour
{
    //public DrawLine drawLine;

    public enum SensorType
    {
        ACCELEROMETER,
        STEPCOUNTER
    }

    public static SensorController Instance;

    private Accelerometer accelerometer;
    private StepCounter stepCounter;

    private LinearAccelerationSensor linearSensor;
    //[SerializeField]
    //private TextMeshProUGUI linearText;
    private GravitySensor gravitySensor;
    //[SerializeField]
    //private TextMeshProUGUI gravityText;

    //[SerializeField]
    //private TextMeshProUGUI accelerometerText;
    //[SerializeField]
    //private TextMeshProUGUI counterText;

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
        Initialize();

        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        //shakeDetectionThreshold *= shakeDetectionThreshold; // squared for avoiding magnitude
        lowPassValue = CurrentLinearAcceleration();
    }

    private void Update()
    {
        //if (accelerometer != null)
        //{
        //    accelerometerText.text = "Accelerometer: " + accelerometer.acceleration.ReadValue();

        //    Vector3 position = new Vector3(0, 3, 0);
        //    drawLine.positionOne = position;
        //    drawLine.positionTwo = position + accelerometer.acceleration.ReadValue();
        //    drawLine.color1 = Color.red;
        //    drawLine.color2 = Color.blue;
        //}
        //if (stepCounter != null)
        //{
        //    counterText.text = "StepCounter: " + stepCounter.stepCounter.ReadValue();
        //}
        //if (linearSensor != null)
        //{
        //    linearText.text = "LinearAccelerationSensor: " + linearSensor.acceleration.ReadValue();
        //}
        //if (gravitySensor != null)
        //{
        //    gravityText.text = "GravitySensor: " + gravitySensor.gravity.ReadValue();
        //}
    }

    #region Permission Callbacks
    public void PermissionCallbacks_PermissionGranted(string permissionName)
    {
        if (permissionName == "ACTIVITY_RECOGNITION")
        {
            InputSystem.EnableDevice(StepCounter.current);
            Debug.Log("StepCounter permission granted and enabled.");
        }
    }

    public void PermissionCallbacks_PermissionDenied(string permissionName)
    {
        if (permissionName == "ACTIVITY_RECOGNITION")
        {
            Debug.Log("StepCounter permission denied.");
        }
    }
    #endregion

    #region Sensor Initialization
    private void InitializeAccelerometer()
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
    }

    private void InitializeStepCounter()
    {
        stepCounter = StepCounter.current;
        if (stepCounter != null)
        {
            InputSystem.EnableDevice(StepCounter.current);
            Debug.Log("StepCounter enabled.");

            // This might not be required if phone has preset permissions for the app
            //if (!Permission.HasUserAuthorizedPermission("ACTIVITY_RECOGNITION"))
            //{
            //    var callbacks = new PermissionCallbacks();
            //    callbacks.PermissionGranted += PermissionCallbacks_PermissionGranted;
            //    callbacks.PermissionDenied += PermissionCallbacks_PermissionDenied;

            //    Permission.RequestUserPermission("ACTIVITY_RECOGNITION", callbacks);
            //}
            //else
            //{
            //    PermissionCallbacks_PermissionGranted("ACTIVITY_RECOGNITION");
            //}
        }
        else
        {
            Debug.Log("StepCounter is null");
        }
    }

    private void InitializeLinearAccelerationSensor()
    {
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
    }

    private void InitializeGravitySensor()
    {
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
    #endregion

    private void Initialize()
    {
        InitializeAccelerometer();
        InitializeStepCounter();
        InitializeLinearAccelerationSensor();
        InitializeGravitySensor();
    }

    int stepCount = 0;
    public int CurrentStepsTaken()
    {
        if (stepCounter == null)
            return stepCount;

        return stepCounter.stepCounter.ReadValue();
    }

    public Vector3 CurrentAcceleration()
    {
        if (accelerometer == null)
            return Vector3.zero;

        return accelerometer.acceleration.ReadValue();
    }

    public Vector3 CurrentLinearAcceleration()
    {
        if (accelerometer == null)
            return Vector3.zero;

        Vector3 acceleration = linearSensor.acceleration.ReadValue();
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        return deltaAcceleration;

        return linearSensor.acceleration.ReadValue();
    }

    public Vector3 CurrentGravity()
    {
        if (gravitySensor == null)
            return Vector3.zero;

        return gravitySensor.gravity.ReadValue();
    }
}
