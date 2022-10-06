using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Item item;

    public virtual void Interact() {
        print("INTERACT");

        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
