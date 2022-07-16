using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffinityBar : MonoBehaviour
{
    private Image heartBar;
    private float maxAffinity = 50;
    public float dog1Affinity;
    public float dog2Affinity;
    public float dog3Affinity;
    private float currentDogAffinity = 10;

    private float rollResult; //need to adapt Anthony's dice roller to work here
    public float rollModifiers; //need to grab this from drunk meter

    private void Start()
    {
        heartBar = this.gameObject.GetComponent<Image>();
        dog1Affinity = 20;
        dog2Affinity = 10;
        dog3Affinity = 5;
        UpdateAffinityBar(1);
    }

    public void OnTestRoll()
    {
        RollAffinity(1);
        Debug.Log("test rolling");
    }

    // temp roller
    private void RollAffinity(int dogTag)
    {
        rollResult = Random.Range(1, 7) + rollModifiers;
        UpdateAffinityBar(dogTag);
    }

    // Call this from the dialogue manager when a dog's affinity is being updated
    public void UpdateAffinityBar(int dogTag)
    {
        // tell dialogue manager which line to output
        if (rollResult < 1)
        {
            Debug.Log("bad roll");
        }
        else
        {
            Debug.Log("good roll");
        }

        // Add roll result to the current dog's affinity meter
        switch (dogTag)
        {
            case 1:
                dog1Affinity += rollResult;
               
                if (dog1Affinity > 50)
                {
                    dog1Affinity = 50;
                }
                else if (dog1Affinity < 0)
                {
                    dog1Affinity = 0;
                }

                currentDogAffinity = dog1Affinity;
                break;

            case 2:
                dog2Affinity += rollResult;
               
                if (dog2Affinity > 50)
                {
                    dog2Affinity = 50;
                }
                else if (dog2Affinity < 0)
                {
                    dog2Affinity = 0;
                }

                currentDogAffinity = dog2Affinity;
                break;

            case 3:
                dog3Affinity += rollResult;
                
                if (dog3Affinity > 50)
                {
                    dog3Affinity = 50;
                }
                else if (dog3Affinity < 0)
                {
                    dog3Affinity = 0;
                }

                currentDogAffinity = dog3Affinity;
                break;
            default:
                Debug.Log("No dog affinity assigned");
                break;
        }

        heartBar.fillAmount = currentDogAffinity / maxAffinity;
        CheckAffinity();
    }

    private void CheckAffinity()
    {
        if (dog1Affinity >= 50 && dog2Affinity >= 50 && dog3Affinity >= 50)
        {
            Debug.Log("maxed out all dogs");
        }
        else if (dog1Affinity >=50 || dog2Affinity >=50 || dog3Affinity >= 50)
        {
            Debug.Log("maxed out one dog");
        }
    }
}
