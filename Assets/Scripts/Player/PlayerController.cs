using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [Header("Camera")]
    public Camera cam;
    public bool lockCursor;

    [Range(0.1f, 10)] public float lookSensitivity;

    public float maxUpRotation;
    public float maxDownRotation;

    private float xRotation = 0;

    [Header("Movement")]
    public CharacterController controller;

    // Speed of forwards and backwards movement
    [Range(0.5f, 20)] public float walkSpeed;

    // Speed of sideways (left and right) movement
    [Range(0.5f, 15)] public float strafeSpeed;

    public KeyCode sprintKey;

    // How many times faster movement along the X and Z axes
    // is when sprinting
    [Range(1, 3)] public float sprintFactor;

    [Range(0.5f, 10)] public float jumpHeight;
    public int maxJumps;

    private Vector3 velocity = Vector3.zero;
    private int jumpsSinceLastLand = 0;

    [Header("Inventory")]
    public InventoryManager inventory;
    public Text pickUp;

    private GameObject currentLookAt;

    void Start() {
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        pickUp.enabled = false;
    }

    void Update() {
        transform.Rotate(0, Input.GetAxis("Mouse X") * lookSensitivity, 0);
        xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -maxUpRotation, maxDownRotation);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        velocity.z = Input.GetAxis("Vertical") * walkSpeed;
        velocity.x = Input.GetAxis("Horizontal") * strafeSpeed;
        velocity = transform.TransformDirection(velocity);

        if (Input.GetKey(sprintKey)) { Sprint(); }

        // Apply manual gravity
        velocity.y += Physics.gravity.y * Time.deltaTime;

        if (controller.isGrounded && velocity.y < 0) { Land(); }

        if (Input.GetButtonDown("Jump")) {
            if (controller.isGrounded) {
                Jump();
            }
            else if (jumpsSinceLastLand < maxJumps) {
                Jump();
            }
        }

        // let item glow when look at it

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3) && hit.collider.CompareTag("Item")) {
            currentLookAt = hit.collider.gameObject;
            Outline outline = currentLookAt.GetComponent<Outline>();
            outline.enabled = true;
            pickUp.text = "Pick up " + currentLookAt.GetComponent<Interactable>().item.name;
            pickUp.enabled = true;
        } else if (Physics.Raycast(ray, out hit, 20) && !hit.collider.CompareTag("Item") && currentLookAt != null) {
            Outline outline = currentLookAt.GetComponent<Outline>();
            outline.enabled = false;
            pickUp.text = "";
            pickUp.enabled = false;
        }

        // Interact with Item
        if (Input.GetKeyDown(KeyCode.F)) {
            Interact();
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private void Sprint() {
        velocity.z *= sprintFactor;
        velocity.x *= sprintFactor;
    }

    private void Jump() {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y);
        jumpsSinceLastLand++;
    }

    private void Land() {
        velocity.y = 0;
        jumpsSinceLastLand = 0;
    }

    private void Interact() {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3)) {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            inventory.itemList.Add(interactable.item);
            interactable.Interact();
            pickUp.text = "";
            pickUp.enabled = false;
        }
    }
}