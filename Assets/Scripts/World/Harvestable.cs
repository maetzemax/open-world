using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    private bool isDestroyed = false;

    private void OnCollisionEnter(Collision collision) {
        print(collision.collider);
    }
}
