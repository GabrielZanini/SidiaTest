using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowValueSlider : MonoBehaviour
{
    public Text label;
    public Slider slider;



    void Update()
    {
        label.text = slider.value.ToString();
    }
}
