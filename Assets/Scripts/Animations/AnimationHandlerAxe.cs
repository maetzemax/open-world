using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandlerAxe : MonoBehaviour
{

    private bool isHarvesting;
    private Animator animator;
    private Camera cam;


    private void Start() {
        animator = gameObject.GetComponent<Animator>();
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isHarvesting) {
            StartCoroutine(HarvestRessource());
        }

        animator.SetBool("isHarvesting", isHarvesting);
    }

    IEnumerator HarvestRessource() {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition); // Mitte Bildschirm
        RaycastHit hit; // Hit Informationen

        if (Physics.Raycast(ray, out hit, 5)) {
            isHarvesting = true;
            yield return new WaitForSeconds(0.5f);
            print("Item collected" + hit.collider.name);
        }
        else {
            isHarvesting = true;
            yield return new WaitForSeconds(0.1f);
        }


        isHarvesting = false;

    }
}
