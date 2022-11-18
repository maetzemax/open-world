using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GenerateMesh))]
public class PlaceObjects : MonoBehaviour {
    public TerrainController TerrainController { get; set; }

    public List<PrefabItem> placeableObjects;
    public List<Vector3> placeableObjectSizes;

    public List<GameObject> grassTypes;

    private void Awake() {
        placeableObjects = PrefabDatabase.instance.prefabItems;

        foreach (var item in placeableObjects) {
            placeableObjectSizes.Add(item.prefabSize);
        }
    }

    public void Place(string terrainName) {
        List<WorldObject> worldObjects = WorldDataManager.instance.worldObjectDB.worldObjects;
        List<WorldObject> filteredObjects = worldObjects.FindAll(e => e.terrainID == terrainName);

        // Plant Grass
        for (int i = 0; i < 20; i++) {
            int prefabType = Random.Range(0, grassTypes.Count);
            Vector3 startPoint = RandomPointAboveTerrain();

            RaycastHit hit;
            if (Physics.Raycast(startPoint, Vector3.down, out hit) &&
                hit.point.y > TerrainController.Beach.transform.position.y &&
                hit.point.y < TerrainController.Mountain.transform.position.y &&
                hit.collider.CompareTag("Terrain")) {
                Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                RaycastHit boxHit;
                if (Physics.BoxCast(startPoint, new Vector3(1, 1, 1), Vector3.down, out boxHit, orientation) &&
                    boxHit.collider.CompareTag("Terrain")) {
                    GameObject grass = grassTypes[prefabType];
                    Vector3 position = new Vector3(startPoint.x, hit.point.y, startPoint.z);
                    Instantiate(grass, position, orientation);
                }
            }
        }
        
        if (filteredObjects.Count == 0) {
            int numObjects = Random.Range(TerrainController.MinObjectsPerTile, TerrainController.MaxObjectsPerTile);

            // Between Water and Beach
            for (int i = 0; i < Random.Range(1, 5); i++) {
                var copyObjects = placeableObjects.FindAll(po => po.prefabID == "log" || po.prefabID == "stick");

                int prefabType = Random.Range(0, copyObjects.Count);
                Vector3 startPoint = RandomPointAboveTerrain();

                RaycastHit hit;
                if (Physics.Raycast(startPoint, Vector3.down, out hit) &&
                    hit.point.y < TerrainController.Beach.transform.position.y &&
                    hit.point.y > TerrainController.Water.transform.position.y && hit.collider.CompareTag("Terrain")) {
                    Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                    RaycastHit boxHit;
                    if (Physics.BoxCast(startPoint, new Vector3(1, 1, 1), Vector3.down, out boxHit, orientation) &&
                        boxHit.collider.CompareTag("Terrain")) {
                        PrefabItem prefabItem = copyObjects[prefabType];
                        Vector3 position = new Vector3(startPoint.x, hit.point.y + .2f, startPoint.z);
                        Instantiate(prefabItem.prefabGameobject, position, orientation, transform);
                    }
                }
            }

            // Between Grass and Mountain
            for (int i = 0; i < numObjects; i++) {
                var copyObjects = placeableObjects.FindAll(po => po.prefabID != "iron_ore" && po.prefabID != "stick" && po.prefabID != "log" && po.prefabID != "altar");

                int prefabType = Random.Range(0, copyObjects.Count);
                Vector3 startPoint = RandomPointAboveTerrain();

                RaycastHit hit;
                if (Physics.Raycast(startPoint, Vector3.down, out hit) &&
                    hit.point.y > TerrainController.Beach.transform.position.y && hit.collider.CompareTag("Terrain")) {
                    Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                    RaycastHit boxHit;
                    if (Physics.BoxCast(startPoint, new Vector3(1, 1, 1), Vector3.down, out boxHit, orientation) &&
                        boxHit.collider.CompareTag("Terrain") &&
                        hit.point.y < TerrainController.Mountain.transform.position.y) {
                        PrefabItem prefabItem = copyObjects[prefabType];
                        Vector3 position = new Vector3(startPoint.x, hit.point.y, startPoint.z);
                        Instantiate(prefabItem.prefabGameobject, position, orientation, transform);
                    }
                }
            }

            // Above Mountain level
            for (int i = 0; i < Random.Range(0, 2); i++) {
                
                var copyObjects = placeableObjects.FindAll(po => po.prefabID == "iron_ore" || po.prefabID == "altar");

                int prefabType = Random.Range(0, copyObjects.Count);
                Vector3 startPoint = RandomPointAboveTerrain();

                RaycastHit hit;
                if (Physics.Raycast(startPoint, Vector3.down, out hit) &&
                    hit.point.y > TerrainController.Mountain.transform.position.y &&
                    hit.collider.CompareTag("Terrain")) {
                    Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                    RaycastHit boxHit;
                    if (Physics.BoxCast(startPoint, placeableObjectSizes[prefabType], Vector3.down, out boxHit,
                            orientation) && boxHit.collider.CompareTag("Terrain") &&
                        hit.point.y > TerrainController.Mountain.transform.position.y) {
                        PrefabItem prefabItem = copyObjects[prefabType];
                        Vector3 position = new Vector3(startPoint.x, hit.point.y, startPoint.z);
                        Instantiate(prefabItem.prefabGameobject, position, orientation, transform);
                    }
                }
            }
        }
        else {
            foreach (var item in filteredObjects) {
                PrefabItem prefabItem =
                    PrefabDatabase.instance.prefabItems.Where(p => p.prefabID == item.prefabID).First();

                RaycastHit hit;
                Ray ray = new(item.worldPosition + new Vector3(0, 100, 0), Vector3.down);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
                    GameObject go = Instantiate(prefabItem.prefabGameobject, item.worldPosition, item.orientation,
                        hit.transform);
                    go.name = prefabItem.prefabID;
                }
            }
        }
    }

    private Vector3 RandomPointAboveTerrain() {
        return new Vector3(
            Random.Range(transform.position.x - TerrainController.TerrainSize.x / 2,
                transform.position.x + TerrainController.TerrainSize.x / 2),
            transform.position.y + TerrainController.TerrainSize.y * 2,
            Random.Range(transform.position.z - TerrainController.TerrainSize.z / 2,
                transform.position.z + TerrainController.TerrainSize.z / 2)
        );
    }
}