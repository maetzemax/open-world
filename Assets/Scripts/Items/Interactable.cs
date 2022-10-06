using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Item item;

    private void Start() {
        Outline outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;
    }

    public virtual void Interact() {
        print("INTERACT");
        Destroy(gameObject);
    }
}
