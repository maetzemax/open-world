using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome : MonoBehaviour
{

    Transform terrainTransform;

    public float radius = 40;

    private void Start() {
        terrainTransform = GetComponent<Transform>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(terrainTransform.position.x, terrainTransform.position.y + radius / 2, terrainTransform.position.z), radius);
    }

    private void LateUpdate() {
        if (gameObject.CompareTag("Swamp")) {
            radius = 40;
        } else {
            radius = 0;
        }
    }
}
