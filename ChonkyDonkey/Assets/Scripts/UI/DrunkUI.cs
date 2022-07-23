using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrunkUI : MonoBehaviour
{
    [SerializeField] Drunkeness drunkeness;
    [SerializeField] Slider drunkMeter;
    [SerializeField] Image drunkMeterFill;
    [SerializeField] TMPro.TextMeshProUGUI rollModifierText;
    [SerializeField] Animation swayAnimation;

    private float oldDrunkPercentage = 0;
    private float currentDrunkPercentage = 0;
    private float animationDrunkPercentage = 0;
    private float lerpInterpolation = 0;
    private readonly float animationSpeed = 2.5f;
    private bool isPendingDrinkMeterAnimation;

    // Start is called before the first frame update
    void Start()
    {
        currentDrunkPercentage = drunkeness.GetDrunkenessPercentage();
        oldDrunkPercentage = currentDrunkPercentage;
        drunkMeter.value = currentDrunkPercentage;
        drunkMeterFill.GetComponent<Image>().color = drunkeness.GetIntoxicationColor();
        rollModifierText.text = GetRollModifierString();
        drunkeness.DrunkenessChanged += OnDrunkenessChanged;
    }

    public void OnDrunkenessChanged(int delta)
    {
        oldDrunkPercentage = currentDrunkPercentage;
        currentDrunkPercentage = drunkeness.GetDrunkenessPercentage();

        isPendingDrinkMeterAnimation = true;
        animationDrunkPercentage = 0;
        lerpInterpolation = 0;
        
        FXOverlayUI.Instance.OnDrunkChanged(delta);
    }

    private string GetRollModifierString()
    {
        string result = "Roll Modifier ";
        int modifier = drunkeness.GetRollModifier();

        if(modifier >= 0)
        {
            result += "+";
        }

        result += modifier;

        return result;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPendingDrinkMeterAnimation)
        {
            AnimateDrunkMeter();
        }
    }

    private void AnimateDrunkMeter()
    {
        lerpInterpolation += animationSpeed * Time.deltaTime;
        animationDrunkPercentage = Mathf.Lerp(oldDrunkPercentage, currentDrunkPercentage, lerpInterpolation);

        drunkMeter.value = animationDrunkPercentage;

        swayAnimation.Play();

        if (animationDrunkPercentage == currentDrunkPercentage)
        {
            drunkMeterFill.color = drunkeness.GetIntoxicationColor();
            rollModifierText.text = GetRollModifierString();
            isPendingDrinkMeterAnimation = false;
        }
    }
}
