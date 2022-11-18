using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HarvestAnimation : MonoBehaviour {

    public string parameterName;
    public Animator animator;

    GameObject invetoryUI;

    private Camera cam;

    private bool isHarvesting;

    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        cam = FindObjectOfType<Camera>();
        invetoryUI = FindObjectOfType<InventoryUI>().inventoryUI;
    }

    void Update() {
        if (!invetoryUI.activeSelf) {
            if (Input.GetMouseButtonDown(0) && !isHarvesting) {
                Harvest();
            }
        }

        animator.SetBool(parameterName, isHarvesting);
    }

    IEnumerator HarvestRessource() {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2) && hit.collider.CompareTag("Ressource")) {
            isHarvesting = true;
            Harvestable harvestable = hit.collider.gameObject.GetComponentInParent<Harvestable>();

            yield return new WaitForSeconds(0.7f);

            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            if (player.selectedTool.item.category == harvestable.destrutable) {
                harvestable.Harvest();
            }
        }
        else {
            isHarvesting = true;
            yield return new WaitForSeconds(0.7f);
        }

        isHarvesting = false;
    }

    public virtual void Harvest() {
        StartCoroutine(HarvestRessource());
    }
}
