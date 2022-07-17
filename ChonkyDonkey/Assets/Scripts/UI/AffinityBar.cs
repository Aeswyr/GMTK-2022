using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffinityBar : MonoBehaviour
{
    private GameObject takeOneHomeCanvas;
    private GameObject takeAllHomeCanvas;
    private static Image heartBar;
    private float maxAffinity = 50;
    public static float dog1Affinity;
    public static float dog2Affinity;
    public static float dog3Affinity;
    private float currentDogAffinity = 10;
    public static int currentDogTag;
    public static bool rolledDice = false;

    private float rollResult; //need to adapt Anthony's dice roller to work here

    [Header("Drink modifiers")]
    private Drunkeness drunkScript;
    //public float rollModifiers; //need to grab this from drunk meter

    public GameObject diceSideChecker;
    public FlipCupGameHandler flipCupHandler;
    public DialogueOverlayUI dialogueScript;


    private void Start()
    {
        takeOneHomeCanvas = GameObject.FindWithTag("TakeOneHomeCanvas");
        takeAllHomeCanvas = GameObject.FindWithTag("TakeAllHomeCanvas");
        heartBar = this.gameObject.GetComponent<Image>();
        dog1Affinity = 10;
        dog2Affinity = 20;
        dog3Affinity = 5;
        drunkScript = FindObjectOfType<Drunkeness>();

        if (takeOneHomeCanvas.activeInHierarchy || takeAllHomeCanvas.activeInHierarchy)
        {
            takeOneHomeCanvas.SetActive(false);
            takeAllHomeCanvas.SetActive(false);
        }
        if (this.gameObject.activeInHierarchy)
        {
            this.gameObject.SetActive(false);
        }
    }
    public int GetDogTag()
    {
        return currentDogTag;
    }

    // call this from the interactable GO when starting a conversation with a dog
    public int ShowThisDogsAffinity(int dogTag)
    {
        currentDogTag = dogTag;

        this.gameObject.SetActive(true);
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

        return (int)currentDogAffinity;
    }

    public void HideAffinity()
    {
        this.gameObject.SetActive(false);
    }

    // call this when updating the affinity bar after a dice roll
    public void UpdateAffinityAfterRoll(int dogTag)
    {
        int rollResult = diceSideChecker.GetComponent<DiceSideCheck>().PlayerRoll() + drunkScript.GetRollModifier();
        rolledDice = false;


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

        // tell dialogue manager which line to output
        if (rollResult < 1)
        {
            dialogueScript.OnFail((PetId)dogTag, (int)currentDogAffinity);
        }
        else
        {
            dialogueScript.OnSuccess((PetId)dogTag, (int)currentDogAffinity);
        }

        heartBar.fillAmount = currentDogAffinity / maxAffinity;
        CheckAffinity();

        StartCoroutine(ShowDice());
    }

    IEnumerator ShowDice()
    {
        yield return new WaitForSeconds(2);
        ModeManager.Instance.ChangeMode(GameMode.Bar);
        //flipCupHandler.ToggleAwooRoll();
    }

    // Check if the player can take the dog home
    private void CheckAffinity()
    {
        if (dog1Affinity >= 50 && dog2Affinity >= 50 && dog3Affinity >= 50)
        {
            takeAllHomeCanvas.SetActive(true);
        }
        else if (dog1Affinity >=50 || dog2Affinity >=50 || dog3Affinity >= 50)
        {
            takeOneHomeCanvas.SetActive(true);
        }
    }

    public void TakeHome()
    {
        Debug.Log("Took dog home. Game over");
    }

    public void KeepSocializing()
    {
        Debug.Log("Didn't take dog home. Continuing game.");
        takeOneHomeCanvas.SetActive(false);
        takeAllHomeCanvas.SetActive(false);
    }
}
