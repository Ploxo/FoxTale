using TMPro;
using UnityEngine;

public class JumpTracker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI reportText;

    [SerializeField]
    float expireTimer = 2f;
    [SerializeField]
    float threshold = 0.75f;
    //[SerializeField]
    //float restThreashold = 0.5f;

    private SensorController sensorController;

    private int count = 0;
    private int jumps = 0;
    private float timer = 0f;

    public int JumpsPerformed => jumps;


    private void Start()
    {
        sensorController = SensorController.Instance;
    }

    private void Update()
    {
        MeasureJump();
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

            if (reportText != null)
                reportText.text = "" + jumps;
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
