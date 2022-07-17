using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : Singleton<ModeManager>
{
    public Camera BarCam;
    public GameObject FlipCup;
    public GameObject Awoo;
    
    public GameMode Mode;

    public void ChangeMode(GameMode newMode)
    {
        Mode = newMode;
        BarCam.gameObject.SetActive(newMode == GameMode.Bar || newMode == GameMode.Dialogue);
        FlipCup.SetActive(newMode == GameMode.CupDice);
        // Awoo.SetActive(newMode == gameMode.AwooDice);
    }
}

public enum GameMode
{
    Bar,
    Dialogue,
    CupDice,
    AwooDice,
}
