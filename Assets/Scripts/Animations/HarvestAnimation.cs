using System.Collections;
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

        if (Physics.Raycast(ray, out hit, 2) && hit.collider.CompareTag("Ressource") ||
            Physics.Raycast(ray, out hit, 2) && hit.collider.CompareTag("Boss")) {
            isHarvesting = true;
            Harvestable harvestable = hit.collider.gameObject.GetComponentInParent<Harvestable>();

            yield return new WaitForSeconds(0.35f);

            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            if (player.selectedTool.item.category == Category.SWORD) {
                SwordAttributes sword = player.toolHolder.GetComponentInChildren<SwordAttributes>();
                GameObject enemy = GameObject.FindGameObjectWithTag("Boss");
                enemy.GetComponent<EnemyBehaviour>().health -= sword.damage;
            }
            else if (player.selectedTool.item.category == harvestable.destrutable) {
                harvestable.Harvest();
            }

            yield return new WaitForSeconds(0.35f);
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