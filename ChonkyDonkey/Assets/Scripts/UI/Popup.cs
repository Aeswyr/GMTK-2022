using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Popup : MonoBehaviour
{
    public TextMeshProUGUI TitleLabel;
    public TextMeshProUGUI DescriptionLabel;
    public TextMeshProUGUI ConfirmLabel;
    public TextMeshProUGUI CancelLabel;

    private UnityAction confirmAction;
    private UnityAction cancelAction;

    public void Show(string title, string description, string confirm, string cancel, UnityAction confirmAction, UnityAction cancelAction)
    {
        gameObject.SetActive(true);
        TitleLabel.text = title;
        DescriptionLabel.text = description;
        ConfirmLabel.text = confirm;
        CancelLabel.text = cancel;
        this.confirmAction = confirmAction;
        this.cancelAction = cancelAction;
    }

    public void OnConfirmPressed()
    {
        confirmAction?.Invoke();
    }

    public void OnCancelPressed()
    {
        cancelAction?.Invoke();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
