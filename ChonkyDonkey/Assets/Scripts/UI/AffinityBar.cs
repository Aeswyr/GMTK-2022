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
    private int currentDogTag;

    private float rollResult; //need to adapt Anthony's dice roller to work here
    public float rollModifiers; //need to grab this from drunk meter

    private void Start()
    {
        heartBar = this.gameObject.GetComponent<Image>();
        dog1Affinity = 20;
        dog2Affinity = 10;
        dog3Affinity = 5;

        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void ToggleUiOn()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    public void OnTestRoll()
    {
        RollAffinity();
        Debug.Log("test rolling");
    }

    // temp roller
    private void RollAffinity()
    {
        rollResult = Random.Range(1, 7) + rollModifiers;
        UpdateAffinityAfterRoll(currentDogTag);
    }
    
    // call this from the dialogue manager when starting a conversation with a dog
    public void ShowThisDogsAffinity(int dogTag)
    {
        switch (dogTag)
        {
            case 1:
                currentDogAffinity = dog1Affinity;
                currentDogTag = 1;
                break;
            case 2:
                currentDogAffinity = dog2Affinity;
                currentDogTag = 2;
                break;
            case 3:
                currentDogAffinity = dog3Affinity;
                currentDogTag = 3;
                break;
            default:
                Debug.Log("Not a datable dog");
                break;
        }

        heartBar.fillAmount = currentDogAffinity / maxAffinity;
    }

    // call this when updating the affinity bar after a dice roll
    public void UpdateAffinityAfterRoll(int dogTag)
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

    // Check if the player can take the dog home
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
