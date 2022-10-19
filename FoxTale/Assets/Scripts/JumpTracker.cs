using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class JumpTracker : MonoBehaviour
{
    [SerializeField]
    private SensorController sensorController;
    //[SerializeField]
    //private TextMeshProUGUI reportText;

    int jumps = 0;
    float timer = 0f;
    float expireTimer = 2f;
    float threshold = 0.75f;
    float restThreashold = 0.5f;
    int count = 0;

    bool cancel = false;

    StringBuilder sb = new StringBuilder();


    private void Update()
    {
        MeasureJump();
    }

    public void OnRecordPressed()
    {
        if (!cancel)
            StartCoroutine(RecordJump());
    }

    public void OnStopRecordPressed()
    {
        cancel = true;
    }

    public void OnSaveRecordingPressed()
    {
        OutputWriter.WriteString("jump_output.txt", sb.ToString(), false);
    }

    private IEnumerator RecordJump()
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

    private void MeasureJump()
    {
        Vector3 linearAcceleration = sensorController.CurrentLinearAcceleration();
        //float linearMag = linearAcceleration.magnitude;
        float dot = Vector3.Dot(linearAcceleration, sensorController.CurrentGravity());

        if (timer > 0f)
            timer -= Time.deltaTime;

        if (count == 0 && dot > threshold)
        {
            timer = expireTimer;
            count++;
            //Debug.Log($"Count: {count}, Jump buildup at {Time.time}");
        }
        //else if (count == 1 && linearMag < restThreashold)
        //{
        //    count++;
        //    //Debug.Log($"Count: {count}, Jump upwards deceleration at {Time.time}");
        //}
        else if (count == 1 && dot < -threshold)
        {
            count++;
            //Debug.Log($"Count: {count}, Jump launch at {Time.time}");
        }
        else if (count == 2 && dot > threshold)
        {
            count++;
            //Debug.Log($"Count: {count}, Jump peak at {Time.time}");
        }

        // Count up. Timer will start on next launch
        if (count == 3)
        {
            count = 0;
            timer = 0f;
            jumps++;
            //reportText.text = "" + jumps;
        }
        else if (count > 0 && timer <= 0)
        {
            ResetJump();
        }
    }

    private void ResetJump()
    {
        //Debug.Log($"Reset");
        count = 0;
    }
}
