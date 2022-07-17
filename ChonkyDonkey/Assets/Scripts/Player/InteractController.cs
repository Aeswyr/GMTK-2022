using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    [SerializeField] private Collider2D interactBox;
    [SerializeField] private GameObject interactEmote;

    private ContactFilter2D filter;
    void Awake() {
        filter.useTriggers = true;
        filter.SetLayerMask(LayerMask.GetMask("Interactable"));
    }
    void OnTriggerEnter2D(Collider2D other) {
        interactEmote.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other) {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        interactBox.Cast(Vector2.zero, filter, hits);

        if (hits.Count == 0)
            interactEmote.SetActive(false);
    }

    public List<Interactable> FindValidInteractables() {
        List<Interactable> interactables = new List<Interactable>();
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        interactBox.Cast(Vector2.zero, filter, hits);

        foreach (var hit in hits) {
            if (hit.collider.gameObject.TryGetComponent(out Interactable interactable))
                interactables.Add(interactable);
        }

        return interactables;
    }
}
