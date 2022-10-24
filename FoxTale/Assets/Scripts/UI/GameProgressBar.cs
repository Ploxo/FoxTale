using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressBar : MonoBehaviour
{
    private Slider bar;

    [SerializeField]
    private float lerpDuration = 0.25f;

    private float targetValue;
    //private float currentValue = 0f;
    private float elapsed = 0f;

    private float startValue = 0f;

    void Start()
    {
        bar = GetComponent<Slider>();

        ResetValue();
    }

    void Update()
    {
        if (bar.value < targetValue)
        {
            //Debug.Log($"called update with bar value {bar.value} and elapsed / lerpDuration {elapsed / lerpDuration}");
            bar.value = Mathf.SmoothStep(startValue, targetValue, elapsed / lerpDuration);
            elapsed += Time.deltaTime;
        }

        if (bar.value >= targetValue)
        {
            elapsed = 0f;
            bar.value = targetValue;
        }

    }

    //private IEnumerator LerpOverSeconds()
    //{

    //    float elapsed = 0;
    //    while (elapsed < lerpDuration)
    //    {
    //        currentValue = Mathf.Lerp(currentValue, targetValue, elapsed / lerpDuration);
    //        elapsed += Time.deltaTime;

    //        yield return null;
    //    }

    //    currentValue = targetValue;
    //}

    public void ResetValue()
    {
        bar.value = 0f;
        targetValue = 0f;
        elapsed = 0f;
    }

    //public void SetMinMax(int min, int max)
    //{
    //    bar.maxValue = max;
    //    bar.minValue = min;
    //}

    public void SetValue(float value)
    {
        startValue = bar.value;
        targetValue = value;
    }
}
