using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection { Left, Right }
    FacingDirection direction;

    private Rigidbody2D rb;

    public float speed;

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    public float apexHeight;
    public float timeToApexInSeconds;

    private float jumpVelocity;
    private float tempVelocity;

    public float gravity;
    public float terminalVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set initial facing direction
        direction = FacingDirection.Right;

        jumpVelocity = 2 * apexHeight / timeToApexInSeconds;
    }

    void FixedUpdate()
    {
        HandleJump();
    }

    public bool IsWalking()
    {
        // Get horizontal input and adjust velocity based on it
        float inputX = PlayerInput.GetDirectionalInput().x;
        rb.velocity = new Vector2(speed * inputX, rb.velocity.y);

        // If there is horizontal input then the player is walking
        if (inputX != 0)
        {
            return true;
        }
        // If the code above was not reached, then the player is not walking
        return false;
    }

    public bool IsGrounded()
    {
        // Cast a ray in the shape of a box underneath player using variables
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public FacingDirection GetFacingDirection()
    {
        float inputX = PlayerInput.GetDirectionalInput().x;
        // Player is facing right
        if(inputX > 0)
        {
            return direction = FacingDirection.Right;
        }
        // Player is facing left
        else if(inputX < 0)
        {
            return direction = FacingDirection.Left;
        }
        // Player is facing same direction they were last time this function was called
        return direction;
    }

    public void HandleJump()
    {
        if (PlayerInput.WasJumpPressed() && IsGrounded())
        {
            tempVelocity = jumpVelocity;
            rb.velocity = new Vector3(rb.velocity.x, tempVelocity);
        }
        else if(!IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, tempVelocity);
            if(IsGrounded())
            {
                rb.velocity = new Vector3(rb.velocity.x, 0);
            }
            else if (tempVelocity >= terminalVelocity)
            {
                tempVelocity += gravity;
            }
        }
    }

    // Makes BoxCast visible in scene view
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }
}
