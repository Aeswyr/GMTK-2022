using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : Singleton<ModeManager>
{
    public Camera BarCam;
    public GameObject FlipCup;

    public void SetModeFlipCup()
    {
        ChangeMode(true);
    }
    
    public void SetModeBar()
    {
        ChangeMode(false);
    }

    private void ChangeMode(bool flipcup)
    {
        BarCam.gameObject.SetActive(!flipcup);
        FlipCup.SetActive(flipcup);
    }
}
