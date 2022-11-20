using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarBehaviour : MonoBehaviour {
    public List<GameObject> shardsPrefabs;
    public GameObject shardHolder;

    // Start is called before the first frame update
    void Start() {
        var randomShard = Random.Range(0, shardsPrefabs.Count);
        Instantiate(shardsPrefabs[randomShard], shardHolder.transform);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (shardHolder == null)
            return;

        shardHolder.transform.Rotate(0, 3f, 0);
    }
}