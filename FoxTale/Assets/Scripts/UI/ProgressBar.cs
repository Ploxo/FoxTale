using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    public int current;

    [SerializeField]
    private Image mask;
    [SerializeField]
    private int max;
    [SerializeField]
    private int min;


    private void Update()
    {
        GetCurrentFill();
    }

    private void GetCurrentFill()
    {
        float offset = current - min;
        float maxOffset = max - min;
        float fillAmount = offset / (float)maxOffset;
        mask.fillAmount = fillAmount;
    }
}
