using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_StatBar : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider slider;
    // Variable to scale up the bar based on Stats
    // Secondary bar to show used or lost bar 

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();    
    }
    public void SetStat(int newValue)
    {
        slider.value = newValue;
    }
    public void SetMaxStat(int maxValue)
    {
        slider.maxValue = maxValue;
        slider.value = maxValue;
    }
}
