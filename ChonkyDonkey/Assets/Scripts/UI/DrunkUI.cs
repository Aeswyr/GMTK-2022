using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrunkUI : MonoBehaviour
{
    [SerializeField] Drunkeness drunkeness;
    [SerializeField] Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = drunkeness.GetDrunkenessPercentage();
    }

    public void onDrunkenessChanged()
    {
        slider.value = drunkeness.GetDrunkenessPercentage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
