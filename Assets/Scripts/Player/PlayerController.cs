using ProceduralToolkit;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public int health = 20;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector] public bool canMove = true;

    private GameObject currentLookAt;

    [HideInInspector] public bool isInventoryOpen = false;
    [HideInInspector] public ItemObject selectedTool = null;

    public GameObject toolHolder;
    public GameObject slider;
    
    private InventoryManager inventory;

    private void Start() {
        inventory = InventoryManager.instance;
        
        characterController = GetComponent<CharacterController>();

        if (!characterController.isGrounded) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit) || Physics.Raycast(transform.position, Vector3.up, out hit) && hit.collider.CompareTag("Terrain")) {
                gameObject.transform.position = new Vector3(transform.position.x, hit.point.y + 0.2f, transform.position.z);
            }
        }
        
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Awake() {
        if (PlayerPrefs.HasKey("x")) {
            var playerRotation = Quaternion.Euler(0, PlayerPrefs.GetFloat("y_rotation"), 0);
            var playerPosition = new Vector3(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"),
                PlayerPrefs.GetFloat("z"));
            gameObject.transform.rotation = playerRotation;
            gameObject.transform.position = playerPosition;

            var camRotation = Quaternion.Euler(PlayerPrefs.GetFloat("x_rotation"), 0, 0);
            playerCamera.transform.rotation = camRotation;
            rotationX = PlayerPrefs.GetFloat("x_rotation");
        }
    }

    void Update() {

        if (health <= 0) {
            Destroy(gameObject);
        }
        
        if (Input.GetButtonDown("Inventory")) {
            isInventoryOpen = !isInventoryOpen;
        }

        Cursor.visible = isInventoryOpen;

        if (isInventoryOpen) {
            Cursor.lockState = CursorLockMode.None;
            canMove = false;
        }
        else {
            canMove = true;
            Cursor.lockState = CursorLockMode.Locked;

            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            // Press Left Shift to run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded) {
                moveDirection.y = jumpSpeed;
            }
            else {
                moveDirection.y = movementDirectionY;
            }

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (!characterController.isGrounded) {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
            if (canMove) {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }

            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3) && hit.collider.CompareTag("Item")) {
                currentLookAt = hit.collider.gameObject;
                inventory.pickUpText.text =
                    "Collect " + currentLookAt.GetComponent<Interactable>().gameObject.name;
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

            if (selectedTool.item != null) {
                toolHolder.GetComponent<HarvestAnimation>().enabled = selectedTool.item.isTool;
                if (toolHolder.transform.childCount > 0)
                    currentSelectedTool = toolHolder.transform.GetChild(0).gameObject;


                if (currentSelectedTool != null &&
                    currentSelectedTool.gameObject.name != selectedTool.item.prefab.name + "(Clone)") {
                    Destroy(currentSelectedTool);

                    if (selectedTool.item.isTool)
                        Instantiate(selectedTool.item.prefab, toolHolder.transform);
                }
                else if (currentSelectedTool == null) {
                    if (selectedTool.item.isTool)
                        Instantiate(selectedTool.item.prefab, toolHolder.transform);
                }
            }
        }

        PlayerPrefs.SetFloat("y_rotation", gameObject.transform.rotation.eulerAngles.y);
        PlayerPrefs.SetFloat("x_rotation", playerCamera.transform.rotation.eulerAngles.x);
        PlayerPrefs.SetFloat("x", gameObject.transform.position.x);
        PlayerPrefs.SetFloat("y", gameObject.transform.position.y);
        PlayerPrefs.SetFloat("z", gameObject.transform.position.z);

        PlayerPrefs.Save();
    }

    private void Interact() {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3) && hit.collider.CompareTag("Item")) {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();
            interactable.Interact();
        }
    }
}