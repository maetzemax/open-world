using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GenerateMesh))]
public class PlaceObjects : MonoBehaviour {

    public TerrainController TerrainController { get; set; }

    public void Place() {
        int numObjects = Random.Range(TerrainController.MinObjectsPerTile, TerrainController.MaxObjectsPerTile);

        // hit.collider.CompareTag(); -> bezieht sich auf den Tag des Tiles
        int randomValue = 10;
        for (int i = 0; i < randomValue; i++) {

            Vector3 startPoint = RandomPointAboveTerrain();

            RaycastHit hit;

            if (Physics.Raycast(startPoint, Vector3.down, out hit) && hit.point.y < TerrainController.Mountain.transform.position.y && hit.point.y > TerrainController.Greenland.transform.position.y && hit.collider.CompareTag("Swamp")) {
                Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                RaycastHit boxHit;
                if (Physics.BoxCast(startPoint, TerrainController.PlaceableObjectSizes[2], Vector3.down, out boxHit, orientation) && boxHit.collider) {
                    Instantiate(TerrainController.PlaceableObjects[2], new Vector3(startPoint.x, hit.point.y, startPoint.z), orientation, transform);
                }
            }
        }

        for (int i = 0; i < numObjects; i++) {
            int prefabType = Random.Range(0, TerrainController.PlaceableObjects.Length);
            Vector3 startPoint = RandomPointAboveTerrain();

            RaycastHit hit;

            // hit.collider.CompareTag(); -> bezieht sich auf den Tag des Tiles


            if (Physics.Raycast(startPoint, Vector3.down, out hit) && hit.point.y > TerrainController.Mountain.transform.position.y && hit.collider.CompareTag("Terrain")) {
                Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                RaycastHit boxHit;
                if (Physics.BoxCast(startPoint, TerrainController.PlaceableObjectSizes[3], Vector3.down, out boxHit, orientation) && boxHit.collider) {
                    Instantiate(TerrainController.PlaceableObjects[3], new Vector3(startPoint.x, hit.point.y, startPoint.z), orientation, transform);
                }
            }


            /*if (Physics.Raycast(startPoint, Vector3.down, out hit) && hit.point.y > TerrainController.Mountain.transform.position.y && (hit.collider.CompareTag("Terrain") || hit.collider.CompareTag("Swamp"))) {
                Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                RaycastHit boxHit;
                if (Physics.BoxCast(startPoint, TerrainController.PlaceableObjectSizes[3], Vector3.down, out boxHit, orientation) && boxHit.collider) {
                    Instantiate(TerrainController.PlaceableObjects[3], new Vector3(startPoint.x, hit.point.y, startPoint.z), orientation, transform);
                }
            }

            if (Physics.Raycast(startPoint, Vector3.down, out hit) && hit.point.y > TerrainController.Forest.transform.position.y && hit.point.y < TerrainController.Mountain.transform.position.y && (hit.collider.CompareTag("Terrain") || hit.collider.CompareTag("Swamp"))) {
                Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                RaycastHit boxHit;
                if (Physics.BoxCast(startPoint, TerrainController.PlaceableObjectSizes[2], Vector3.down, out boxHit, orientation) && boxHit.collider) {
                    Instantiate(TerrainController.PlaceableObjects[2], new Vector3(startPoint.x, hit.point.y, startPoint.z), orientation, transform);
                }
            }

            if (Physics.Raycast(startPoint, Vector3.down, out hit) && hit.point.y > TerrainController.Greenland.transform.position.y && hit.point.y < TerrainController.Forest.transform.position.y && (hit.collider.CompareTag("Terrain") || hit.collider.CompareTag("Swamp"))) {
                Quaternion orientation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                RaycastHit boxHit;
                if (Physics.BoxCast(startPoint, TerrainController.PlaceableObjectSizes[1], Vector3.down, out boxHit, orientation) && boxHit.collider) {
                    Instantiate(TerrainController.PlaceableObjects[1], new Vector3(startPoint.x, hit.point.y, startPoint.z), orientation, transform);
                }
            }*/

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