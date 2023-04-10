using UnityEngine;

public class SliderController : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnSliderValueChanged(float value)
    {
        Debug.Log("Slider value changed to " + value);
    }
}
