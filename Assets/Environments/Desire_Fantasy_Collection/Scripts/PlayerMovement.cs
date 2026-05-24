using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float movementSpeed = 7f;
    private Vector2 movement;

    private float horizontal = 0.0f;
    private float vertical = 0.0f;
    private float speed = 0.0f;

    private Rigidbody2D rb;
    private Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        horizontal = movement.x > 0.01f ? movement.x : movement.x < -0.01f ? movement.x : 0;
        vertical = movement.y > 0.01f ? movement.y : movement.y < -0.01f ? movement.y : 0;
        speed = Mathf.Abs(movement.x) + Mathf.Abs(movement.y);

        anim.SetFloat("Horizontal", horizontal);
        anim.SetFloat("Vertical", vertical);
        anim.SetFloat("Speed", speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * movementSpeed * Time.fixedDeltaTime);
    }
}
