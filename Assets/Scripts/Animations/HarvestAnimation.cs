using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestAnimation : MonoBehaviour {

    public string parameterName;
    public Animator animator;

    private Camera cam;

    private bool isHarvesting;

    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        cam = FindObjectOfType<Camera>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && !isHarvesting) {
            Harvest();
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

            harvestable.Harvest();
        }
        else {
            isHarvesting = true;
            yield return new WaitForSeconds(0.1f);
        }

        isHarvesting = false;
    }

    public virtual void Harvest() {
        StartCoroutine(HarvestRessource());
    }
}
