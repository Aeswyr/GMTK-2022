using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AffinityBar : MonoBehaviour
{
    private int maxAffinity = 50;
    public int kiefyAffinity;
    public int umbreonAffinity;
    public int leafeonAffinity;
    private int currentDogAffinity = 10;
    public static int currentDogTag;
    public static bool rolledDice = false;

    private float rollResult; //need to adapt Anthony's dice roller to work here

    [Header("Drink modifiers")]
    private Drunkeness drunkScript;
    //public float rollModifiers; //need to grab this from drunk meter

    public GameObject diceSideChecker;
    public FlipCupGameHandler flipCupHandler;
    public DialogueOverlayUI dialogueScript;
    public Image heartBar1;
    public Image heartBar2;
    public GameObject takeOneHomeCanvas;
    public GameObject takeAllHomeCanvas;

    private void Start()
    {
        umbreonAffinity = 3;
        leafeonAffinity = 3;
        kiefyAffinity = 3;
        drunkScript = FindObjectOfType<Drunkeness>();

        if (takeOneHomeCanvas.activeInHierarchy || takeAllHomeCanvas.activeInHierarchy)
        {
            takeOneHomeCanvas.SetActive(false);
            takeAllHomeCanvas.SetActive(false);
        }
        this.gameObject.SetActive(false);
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
        currentDogTag = dogTag;
        ref int refAffinity = ref GetAffinity(dogTag);
        currentDogAffinity = refAffinity;

        UpdateAffinityBars();

        return currentDogAffinity;
    }

    private ref int GetAffinity(int dogTag)
    {
        switch (dogTag)
        {
            case (int)PetId.Kiefy:
                return ref kiefyAffinity;
            case (int)PetId.Umbreon:
                return ref umbreonAffinity;
            case (int)PetId.Leafeon: 
                return ref leafeonAffinity;
            default:
                Debug.LogError("Not a datable dog");
                return ref currentDogAffinity;
        }
    }

    private void UpdateAffinityBars()
    {
        // one heart per affinity, split onto two rows
        heartBar1.fillAmount = currentDogAffinity / (maxAffinity*.5f);
        heartBar2.fillAmount = (currentDogAffinity-maxAffinity*.5f) / (maxAffinity*.5f);
    }

    public void HideAffinity()
    {
        this.gameObject.SetActive(false);
    }

    // call this when updating the affinity bar after a dice roll
    public void UpdateAffinityAfterRoll(int dogTag)
    {
        int rollResult = diceSideChecker.GetComponent<DiceSideCheck>().GetPlayerRoll() + drunkScript.GetRollModifier();
        rolledDice = false;


        // Add roll result to the current dog's affinity meter
        ref int refAffinity = ref GetAffinity(dogTag);
        refAffinity += rollResult;
        if (refAffinity > 50) refAffinity = 50;
        if (refAffinity < 0) refAffinity = 0;
        currentDogAffinity = refAffinity;

        // tell dialogue manager which line to output
        if (rollResult < 1)
        {
            dialogueScript.OnFail((PetId)dogTag, (int)currentDogAffinity);
        }
        else
        {
            dialogueScript.OnSuccess((PetId)dogTag, (int)currentDogAffinity);
        }

        UpdateAffinityBars();
        CheckAffinity();

        // show fx
        FXOverlayUI.Instance.OnAffinityChanged(rollResult);
    }

    // Check if the player can take the dog home
    private void CheckAffinity()
    {
        if (umbreonAffinity >= 50 && leafeonAffinity >= 50 && kiefyAffinity >= 50)
        {
            takeAllHomeCanvas.SetActive(true);
        }
        else if (currentDogAffinity >= 50)
        {
            takeOneHomeCanvas.SetActive(true);
        }
    }

    public void TakeHome()
    {
        Debug.Log("Took dog home. Game over");
        EndingScreenUI.Instance.Show(EndingScreenUI.EndingResult.AwooOne, (PetId)currentDogTag); 
    }

    public void TakeAllHome()
    {
        Debug.Log("Took all dogs home. Game over");
        EndingScreenUI.Instance.Show(EndingScreenUI.EndingResult.AwooAll, (PetId)currentDogTag);
    }

    public void KeepSocializing()
    {
        Debug.Log("Didn't take dog home. Continuing game.");
        takeOneHomeCanvas.SetActive(false);
        takeAllHomeCanvas.SetActive(false);
    }
}
