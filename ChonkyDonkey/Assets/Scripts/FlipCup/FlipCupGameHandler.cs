using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCupGameHandler : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject HUD;
    public GameObject Overlay;

    public GameObject FlipCupCamera;
    public GameObject FlipCup;
    public GameObject Cups;

    public GameObject AwooRollCamera;
    public GameObject AwooRoll;

    public GameObject FlipCupHUD;

    private void Start()
    {
        MainCamera.SetActive(true);
        HUD.SetActive(true);
        Overlay.SetActive(true);
        FlipCupCamera.SetActive(false);
        FlipCup.SetActive(false);
        FlipCupHUD.SetActiveFast(false);
    }

    public void ToggleFlipCup()
    {
        MainCamera.SetActive(!MainCamera.activeInHierarchy);
        HUD.SetActive(!HUD.activeInHierarchy);
        Overlay.SetActive(!Overlay.activeInHierarchy);
        FlipCupCamera.SetActive(!FlipCupCamera.activeInHierarchy);
        FlipCup.SetActive(!FlipCup.activeInHierarchy);
        FlipCupHUD.SetActiveFast(!FlipCupHUD.activeInHierarchy);

        if (FlipCupHUD.activeInHierarchy)
        {
            TMPro.TextMeshProUGUI tmText = GameObject.FindWithTag("FlipCupHUD").GetComponent<TMPro.TextMeshProUGUI>();
            tmText.text = "Click the white dice to begin.\n\nWhen you roll a six, click on a cup to drink! Try and roll the most sixes before your two opponents! The player who drinks the most cups will win more dice!";
        }

        FlipCupGameStats.resetOnNewPlay();
    }

    public void ToggleAwooRoll()
    {
        MainCamera.SetActive(!MainCamera.activeInHierarchy);
        //HUD.SetActive(!HUD.activeInHierarchy);
        Overlay.SetActive(!Overlay.activeInHierarchy);
        AwooRollCamera.SetActive(!AwooRollCamera.activeInHierarchy);
        AwooRoll.SetActive(!AwooRoll.activeInHierarchy);
    }
}