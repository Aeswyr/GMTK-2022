using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent action;
    [SerializeField] private Material outline;
    private SpriteRenderer sprite;
    void Awake() {
        transform.parent.TryGetComponent<SpriteRenderer>(out sprite);
    }
    public void Run() {
        action.Invoke();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (sprite == null)
            return;

        var m = new List<Material>();
        sprite.GetMaterials(m);
        m.Insert(1, outline);
        sprite.materials = m.ToArray();
    }

    void OnTriggerExit2D(Collider2D other) {
        if (sprite == null)
            return;
        
        var m = new List<Material>();
        sprite.GetMaterials(m);
        m.RemoveAt(1);
        sprite.materials = m.ToArray();
    }
}
