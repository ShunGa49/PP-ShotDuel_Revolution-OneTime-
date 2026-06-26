using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ConfigSlider : MonoBehaviour
{
    [SerializeField] private bool isFloatValue = false;
    [SerializeField] private Text valueTxt;
    private Slider slider; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFloatValue)
        {
            valueTxt.text = slider.value.ToString("F2");
        }
        else
        {
            valueTxt.text = Mathf.Floor(slider.value).ToString();  // êÿÇËéÃÇƒ
        }
    }
}
