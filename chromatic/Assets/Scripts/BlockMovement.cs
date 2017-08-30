using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    public float movementSpeed = 10.0f;          // How quickly the blocks travel up the screen.

    private Rigidbody2D rb;                      // The 2d rigidbody used for moving the line of blocks.

    /* Use this for initialization. */
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /* FixedUpdate is called once per physics tick. */
    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + Vector3.up * movementSpeed * Time.fixedDeltaTime);
    }

    /* Returns the block's current movement speed. */
    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    /* Sets the block's movement speed. */
    public void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }
}