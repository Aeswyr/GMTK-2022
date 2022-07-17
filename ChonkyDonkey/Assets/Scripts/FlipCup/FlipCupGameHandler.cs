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

    private void Start()
    {
        MainCamera.SetActive(true);
        HUD.SetActive(true);
        Overlay.SetActive(true);
        FlipCupCamera.SetActive(false);
        FlipCup.SetActive(false);
    }

    public void ToggleFlipCup()
    {
        MainCamera.SetActive(!MainCamera.activeInHierarchy);
        HUD.SetActive(!HUD.activeInHierarchy);
        Overlay.SetActive(!Overlay.activeInHierarchy);
        FlipCupCamera.SetActive(!FlipCupCamera.activeInHierarchy);
        FlipCup.SetActive(!FlipCup.activeInHierarchy);
        FlipCupGameStats.resetOnNewPlay();
    }
}
