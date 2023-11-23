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

    public float jumpVelocity;
    public float gravity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set initial facing direction
        direction = FacingDirection.Right;
    }

    public bool IsWalking()
    {
        // Get horizontal input and adjust velocity based on it
        float inputX = PlayerInput.GetDirectionalInput().x;
        rb.velocity = new Vector2(speed * inputX, 0);

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
        return false;
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

    public void isJumping()
    {
        if(PlayerInput.WasJumpPressed()) // and player is on the ground
        {
            // set bool to true (now jumping)
        }
        // instantly set rb.velocity.y as jumpVelocity
        // if on the ground, set jump bool to false meaning that calculations stop
        // next, add gravity to jumpVelocity variable if not on the ground
    }

    // Makes BoxCast visible in scene view
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }
}
