using UnityEngine;

public class FlipCupGameHandler : MonoBehaviour
{
    public GameObject HUD;
    public GameObject Overlay;

    public GameObject FlipCupCamera;
    public GameObject FlipCup;
    public GameObject Cups;

    public GameObject AwooRollCamera;
    public GameObject AwooRoll;

    private void Start()
    {
        HUD.SetActive(true);
        FlipCupCamera.SetActive(false);
        FlipCup.SetActive(false);
        FlipCupHUD.Instance.gameObject.SetActiveFast(false);
    }

    public void ActivateFlipCup()
    {
        ToggleFlipCup(true);
    }

    public void DeactivateFlipCup()
    {
        ToggleFlipCup(false);
    }

    private void ToggleFlipCup(bool active)
    {
        Cups.SetActive(active);
        HUD.SetActive(!active);
        FlipCupCamera.SetActive(active);
        FlipCup.SetActive(active);
        FlipCupHUD.Instance.gameObject.SetActiveFast(active);

        if (active)
        {
            FlipCupGameStats.resetOnNewPlay();
            Cups.GetComponent<CupStorage>().restoreCups();
            FlipCupHUD.Instance.OnGameStart();
        }
    }

    public void ToggleAwooRoll()
    {
        //HUD.SetActive(!HUD.activeInHierarchy);
        //Overlay.SetActive(!Overlay.activeInHierarchy);
        AwooRollCamera.SetActive(!AwooRollCamera.activeInHierarchy);
        AwooRoll.SetActive(!AwooRoll.activeInHierarchy);
    }
}