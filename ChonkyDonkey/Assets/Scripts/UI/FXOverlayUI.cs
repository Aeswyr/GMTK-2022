using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FXOverlayUI : Singleton<FXOverlayUI>
{
    public Animator Controller;
    public TextMeshProUGUI AffinityLabel;
    public TextMeshProUGUI DrunkLabel;
    private static readonly int DrunkChanged = Animator.StringToHash("DrunkChanged");
    private static readonly int PlusAffinity = Animator.StringToHash("PlusAffinity");
    private static readonly int MinusAffinity = Animator.StringToHash("MinusAffinity");

    public void OnDrunkChanged(int delta)
    {
        Controller.SetTrigger(DrunkChanged);
        DrunkLabel.text = (delta > 0 ? "+" : "") + delta;
    }

    public void OnAffinityChanged(int signedDelta)
    {
        if (signedDelta > 0)
        {
            Controller.SetTrigger(PlusAffinity);
            AffinityLabel.text = "+" + signedDelta;
        }
        else
        {
            Controller.SetTrigger(MinusAffinity);
            AffinityLabel.text = "" + signedDelta; // signed
        }
    }
}
