using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : Singleton<ModeManager>
{
    public Camera BarCam;
    public FlipCupGameHandler FlipCup;
    public FlipCupGameHandler Awoo;
    
    public GameMode Mode;

    public void ChangeMode(GameMode newMode)
    {
        if (Mode != newMode && (Mode == GameMode.CupDice || newMode == GameMode.CupDice)) FlipCup.ToggleFlipCup();
        if (Mode != newMode && (Mode == GameMode.AwooDice || newMode == GameMode.AwooDice))
            Awoo.ToggleAwooRoll();
        BarCam.gameObject.SetActive(newMode == GameMode.Bar || newMode == GameMode.Dialogue);
        //FlipCup.gameObject.SetActive(newMode == GameMode.CupDice);



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
