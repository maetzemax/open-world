using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    private GameObject player;
    private PlayerController playerController;
    private Rigidbody rb;
    private Animator animator;

    public int health = 50;
    public int attackDamage;
    public float attackRate;
    public float attackRange;
    private float timer = 0;

    public float movementSpeed;
    private float journeyLength;

    void FixedUpdate() {
        var step = movementSpeed * Time.deltaTime;

        if (timer >= attackRate && Vector3.Distance(player.transform.position, transform.position) < attackRange) {
            animator.SetTrigger("attack");
            playerController.health -= attackDamage;
            timer = 0;
        }
        else if (Vector3.Distance(player.transform.position, transform.position) < 50 && !(Vector3.Distance(player.transform.position, transform.position) < attackRange)) {
            animator.SetBool("isMoving", true);

            Vector3 position = Vector3.MoveTowards(transform.position, player.transform.position, step);

            rb.MovePosition(position);
            transform.LookAt(player.transform);
        }
        else {
            animator.SetBool("isMoving", false);
        }

        timer += Time.fixedDeltaTime;
    }

    void Start() {
        player = GameObject.FindWithTag("Player");
        rb = gameObject.GetComponentInChildren<Rigidbody>();
        animator = gameObject.GetComponentInChildren<Animator>();
        playerController = player.GetComponent<PlayerController>();
    }
}