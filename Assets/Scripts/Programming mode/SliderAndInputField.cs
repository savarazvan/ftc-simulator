using UnityEngine;
using UnityEngine.UI;
using System;

public class SliderAndInputField : MonoBehaviour
{
    private InputField input;
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponentInChildren<InputField>();
        slider = GetComponentInChildren<Slider>();
    }

    public void updateSlider()
    {
        int inputValue = Int32.Parse(input.text);
        if (inputValue > 180)
        {
            inputValue = 180;
            input.text = inputValue.ToString();
        }

        else if (inputValue < 0)
        {
            inputValue = 0;
            input.text = inputValue.ToString();

        }

        slider.value=inputValue;
    }

    public void updateInput()
    {
        input.text = slider.value.ToString();
    }
}
