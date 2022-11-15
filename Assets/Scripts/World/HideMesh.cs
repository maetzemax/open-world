using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMesh : MonoBehaviour {
    const float Threshold = 50f; // Some distance away from the player
    private GameObject player; // Drop the player on here from the editor, or find using a tag, name, etc.
    Renderer renderer;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        renderer = gameObject.GetComponentInChildren<Renderer>();

        renderer.enabled = false;
    }

    void FixedUpdate() {

        // Get the absolute distance between the object and the player
        var distanceToPlayer = Mathf.Abs(Vector3.Distance(transform.position, player.transform.position));
        // If greater than threshold, goodbye

        if (distanceToPlayer > Threshold) {

            renderer.enabled = false;

        } else {

            renderer.enabled = true;

        }
    }
}