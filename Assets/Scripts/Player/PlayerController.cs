using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
    [Header("Camera")] public Camera cam;
    public bool lockCursor;

    [Range(0.1f, 10)] public float lookSensitivity;

    public float maxUpRotation;
    public float maxDownRotation;

    private float xRotation = 0;

    [Header("Movement")] public CharacterController controller;

    // Speed of forwards and backwards movement
    [Range(0.5f, 20)] public float walkSpeed;

    // Speed of sideways (left and right) movement
    [Range(0.5f, 15)] public float strafeSpeed;

    public KeyCode sKey;

    // How many times faster movement along the X and Z axes
    // is when sing
    [Range(1, 3)] public float sFactor;

    [Range(0.5f, 10)] public float jumpHeight;
    public int maxJumps;

    private Vector3 velocity = Vector3.zero;
    private int jumpsSinceLastLand = 0;

    private GameObject currentLookAt;

    [HideInInspector] public bool isInventoryOpen = false;
    [HideInInspector] public ItemObject selectedTool = null;

    public GameObject toolHolder;

    InventoryManager inventory;

    private void Start() {
        inventory = InventoryManager.instance;
    }

    void Awake() {
        if (PlayerPrefs.HasKey("y")) {
            var playerRotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("y_rotation"), 0);
            var playerPosition = new Vector3(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"),
                PlayerPrefs.GetFloat("z"));
            gameObject.transform.rotation = playerRotation;
            gameObject.transform.position = playerPosition;

            var camRotation = Quaternion.Euler(PlayerPrefs.GetFloat("x_rotation"), 0, 0);
            cam.transform.rotation = camRotation;
            xRotation = PlayerPrefs.GetFloat("x_rotation");
        }
    }

    void Update() {
        if (Input.GetButtonDown("Inventory")) {
            isInventoryOpen = !isInventoryOpen;
        }

        Cursor.visible = isInventoryOpen;

        if (isInventoryOpen) {
            Cursor.lockState = CursorLockMode.None;

            velocity.z = 0;
            velocity.x = 0;
        }
        else {
            Cursor.lockState = CursorLockMode.Locked;

            transform.Rotate(0, Input.GetAxis("Mouse X") * lookSensitivity, 0);
            xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;
            xRotation = Mathf.Clamp(xRotation, -maxUpRotation, maxDownRotation);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            velocity.z = Input.GetAxis("Vertical") * walkSpeed;
            velocity.x = Input.GetAxis("Horizontal") * strafeSpeed;
            velocity = transform.TransformDirection(velocity);

            // Apply manual gravity
            if (Input.GetKey(sKey)) {
                S();
            }

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
                inventory.pickUpText.text =
                    "Pick up " + currentLookAt.GetComponent<Interactable>().itemObject.item.name;
                inventory.pickUpText.enabled = true;
            }
            else if (Physics.Raycast(ray, out hit, 20) && !hit.collider.CompareTag("Item") && currentLookAt != null) {
                inventory.pickUpText.text = "";
                inventory.pickUpText.enabled = false;
            }

            // Interact with Item
            if (Input.GetKeyDown(KeyCode.F)) {
                Interact();
            }

            GameObject currentSelectedTool = null;

            if (selectedTool == null) {
                if (toolHolder.transform.childCount > 0) {
                    currentSelectedTool = toolHolder.transform.GetChild(0).gameObject;

                    Destroy(currentSelectedTool);
                    toolHolder.GetComponent<HarvestAnimation>().enabled = false;
                }
            }

            if (selectedTool != null) {
                toolHolder.GetComponent<HarvestAnimation>().enabled = selectedTool.item.isTool;
                if (toolHolder.transform.childCount > 0)
                    currentSelectedTool = toolHolder.transform.GetChild(0).gameObject;


                if (currentSelectedTool != null && currentSelectedTool.gameObject.name != selectedTool.item.prefab.name + "(Clone)") {
                    print("create new");

                    Destroy(currentSelectedTool);

                    if (selectedTool.item.isTool)
                        Instantiate(selectedTool.item.prefab,
                            toolHolder.transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity,
                            toolHolder.transform);
                }
                else if (currentSelectedTool == null) {
                    if (selectedTool.item.isTool)
                        Instantiate(selectedTool.item.prefab, toolHolder.transform);
                }
            }
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;

        if (controller.isGrounded && velocity.y < 0) {
            Land();
        }

        PlayerPrefs.SetFloat("y_rotation", gameObject.transform.rotation.eulerAngles.y);
        PlayerPrefs.SetFloat("x_rotation", cam.transform.rotation.eulerAngles.x);
        PlayerPrefs.SetFloat("x", gameObject.transform.position.x);
        PlayerPrefs.SetFloat("y", gameObject.transform.position.y);
        PlayerPrefs.SetFloat("z", gameObject.transform.position.z);

        PlayerPrefs.Save();

        controller.Move(velocity * Time.deltaTime);
    }

    private void S() {
        velocity.z *= sFactor;
        velocity.x *= sFactor;
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

        if (Physics.Raycast(ray, out hit, 3) && hit.collider.CompareTag("Item")) {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            interactable.Interact();
        }
    }
}