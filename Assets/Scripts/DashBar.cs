using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DashBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxDash(float value) {
        slider.maxValue = value;
        slider.value = value;
    }
    public void SetDashValue(float value) {
        slider.value = value;
    }
}
