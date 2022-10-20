using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SensorRecorder : MonoBehaviour
{
    private SensorController sensorController;

    private bool cancel = false;
    private StringBuilder sb = new StringBuilder();


    private void Start()
    {
        sensorController = SensorController.Instance;
    }

    public void OnRecordPressed()
    {
        if (!cancel)
            StartCoroutine(RecordSensors());
    }

    public void OnStopRecordPressed()
    {
        cancel = true;
    }

    public void OnSaveRecordingPressed()
    {
        OutputWriter.WriteString("sensor_output.txt", sb.ToString(), false);
    }

    private IEnumerator RecordSensors()
    {
        bool cancel = false;
        float startTime = Time.time;
        while (!cancel)
        {
            Vector3 lacc = sensorController.CurrentLinearAcceleration();
            Vector3 acc = sensorController.CurrentAcceleration();
            Vector3 grav = sensorController.CurrentGravity();
            float mag = (float)Math.Round(lacc.magnitude, 2);
            float dot = (float)Math.Round(Vector3.Dot(lacc, sensorController.CurrentGravity()), 2);

            float lx = (float)Math.Round(lacc.x, 2);
            float ly = (float)Math.Round(lacc.y, 2);
            float lz = (float)Math.Round(lacc.z, 2);

            float x = (float)Math.Round(acc.x, 2);
            float y = (float)Math.Round(acc.y, 2);
            float z = (float)Math.Round(acc.z, 2);

            float g = (float)Math.Round(grav.magnitude, 2);

            sb.Append($"{Math.Round(Time.time - startTime, 2)}:{mag}:{dot}:{lx}:{ly}:{lz}:{x}:{y}:{z}:{g}\n");

            yield return null;
        }

        cancel = false;
        sb.Append("\n");

        yield return null;
    }
}
