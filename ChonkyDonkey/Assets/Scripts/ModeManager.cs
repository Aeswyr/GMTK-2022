using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : Singleton<ModeManager>
{
    public Camera BarCam;
    public FlipCupGameHandler FlipCup;
    public GameObject Awoo;
    
    public GameMode Mode;

    public void ChangeMode(GameMode newMode)
    {
        if (Mode != newMode && (Mode == GameMode.CupDice || newMode == GameMode.CupDice)) FlipCup.ToggleFlipCup();
        BarCam.gameObject.SetActive(newMode == GameMode.Bar || newMode == GameMode.Dialogue);
        //FlipCup.gameObject.SetActive(newMode == GameMode.CupDice);
        // Awoo.SetActive(newMode == gameMode.AwooDice);
        
        Mode = newMode;
    }
}

public enum GameMode
{
    Bar,
    Dialogue,
    CupDice,
    AwooDice,
}
