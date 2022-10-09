using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GenerateMesh))]
public class PlaceObjects : MonoBehaviour {

    public TerrainController TerrainController { get; set; }

    public List<PrefabItem> placeableObjects;
    public List<Vector3> placeableObjectSizes;

    private void Awake() {
        placeableObjects = PrefabDatabase.instance.prefabItems;

        foreach (var item in placeableObjects) {
            placeableObjectSizes.Add(item.prefabSize);
        }
    }

    public void Place() {
        int numObjects = Random.Range(TerrainController.MinObjectsPerTile, TerrainController.MaxObjectsPerTile);
        for (int i = 0; i < numObjects; i++) {
            int prefabType = Random.Range(0, placeableObjects.Count - 1);
            Vector3 startPoint = RandomPointAboveTerrain();

            RaycastHit hit;
            if (Physics.Raycast(startPoint, Vector3.down, out hit) && hit.point.y > TerrainController.Water.transform.position.y && hit.collider.CompareTag("Terrain")) {
                Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                RaycastHit boxHit;
                if (Physics.BoxCast(startPoint, placeableObjectSizes[0], Vector3.down, out boxHit, orientation) && boxHit.collider.CompareTag("Terrain")) {
                    PrefabItem prefabItem = PrefabDatabase.instance.prefabItems[0];
                    Vector3 position = new Vector3(startPoint.x, hit.point.y, startPoint.z);
                    Instantiate(prefabItem.prefabGameobject, position, orientation, transform);
                    //DataManager.instance.AddWorldObject(new WorldObject(prefabItem.prefabID, boxHit.collider.gameObject.name, position, orientation));
                }
            }

        }
    }

    private Vector3 RandomPointAboveTerrain() {
        return new Vector3(
            Random.Range(transform.position.x - TerrainController.TerrainSize.x / 2, transform.position.x + TerrainController.TerrainSize.x / 2),
            transform.position.y + TerrainController.TerrainSize.y * 2,
            Random.Range(transform.position.z - TerrainController.TerrainSize.z / 2, transform.position.z + TerrainController.TerrainSize.z / 2)
        );
    }
}