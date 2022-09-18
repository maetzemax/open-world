using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovementController : MonoBehaviour
{

    public GameObject player;

    public float movementSpeed = 5.0f;
    public float jumpSpeed = 5.0f;

    private bool isGrounded = true;

    // Update is called once per frame
    void FixedUpdate()
    {

        float verticalMoveInput = Input.GetAxis("Vertical");
        Vector3 verticalMovement = Vector3.forward * verticalMoveInput * movementSpeed * Time.deltaTime;

        float horizontalMoveInput = Input.GetAxis("Horizontal");
        Vector3 horizontalMovement = Vector3.right * horizontalMoveInput * movementSpeed * Time.deltaTime;

        Vector3 jumpMovement = Vector3.up * jumpSpeed * 9.81f;

        if (verticalMoveInput != 0) {
            player.transform.Translate(verticalMovement);
        }

        if (horizontalMoveInput != 0) {
            player.transform.Translate(horizontalMovement);
        }

        if (isGrounded && Input.GetKey(KeyCode.Space)) {
            print("JUMPED");
            player.GetComponent<Rigidbody>().AddForce(jumpMovement);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 3 && !isGrounded) {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision) {
        if (collision.gameObject.layer == 3 && isGrounded) {
            isGrounded = false;
        }
    }
}
