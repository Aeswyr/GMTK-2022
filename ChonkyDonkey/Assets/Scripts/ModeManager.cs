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
        // Cup Dice
        if (newMode == GameMode.CupDice)
        {
            FlipCup.ActivateFlipCup();
        }
        else if (Mode == GameMode.CupDice)
        {
            FlipCupResultOverlay.Instance.Show();
            FlipCup.DeactivateFlipCup();
        }
        
        // Awoo Dice
        if (newMode == GameMode.AwooDice)
        {
            Awoo.ToggleAwooRoll();
        }
        else if (Mode == GameMode.AwooDice)
        {
            Awoo.ToggleAwooRoll();
        }
        
        // Bar + Dialogue
        BarCam.gameObject.SetActive(newMode == GameMode.Bar || newMode == GameMode.Dialogue);

        // change mode
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
