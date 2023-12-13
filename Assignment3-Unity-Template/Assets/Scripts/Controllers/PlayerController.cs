using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection { Left, Right }
    FacingDirection direction;

    private Rigidbody2D rb;

    public float speed;
    private float activeSpeed;

    [Header("Dash Settings")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;

    private float dashCounter;
    private float dashCooldownCounter;

    private bool isDashing;

    [Header("Boxcast Settings")]
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    [Header("Jump Settings")]
    public float apexHeight;
    public float timeToApexInSeconds;

    private float jumpVelocity;
    private float tempVelocity;

    public float gravity;
    public float terminalVelocity;

    private bool canDoubleJump;
    private bool doubleJumped;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Default speed is non-dashing speed
        activeSpeed = speed;

        // Set initial facing direction
        direction = FacingDirection.Right;

        jumpVelocity = 2 * apexHeight / timeToApexInSeconds;
    }

    void FixedUpdate()
    {
        HandleJump();

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dashCooldownCounter <= 0 && dashCounter <= 0)
            {
                // Change speed to dash speed
                activeSpeed = dashSpeed;
                //Activate countdown for dash duration
                dashCounter = dashDuration;

                isDashing = true;
            }
        }

        // If dash duration is ongoing
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            // Player dashed for dash duration
            if (dashCounter <= 0)
            {
                // Set speed back to normal
                activeSpeed = speed;
                // Activate cooldown
                dashCooldownCounter = dashCooldown;

                isDashing = false;
            }
        }

        // Dash cooldown
        if (dashCooldownCounter > 0)
        {
            dashCooldownCounter -= Time.deltaTime;
        }
    }

    public bool IsWalking()
    {
        // Get horizontal input and adjust velocity based on it
        float inputX = PlayerInput.GetDirectionalInput().x;
        rb.velocity = new Vector2(activeSpeed * inputX, rb.velocity.y);
        
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

    private void HandleJump()
    {
        if (PlayerInput.WasJumpPressed())
        {
            if (IsGrounded())
            {
                tempVelocity = jumpVelocity;
                rb.velocity = new Vector3(rb.velocity.x, tempVelocity);
                // Player will now be able to double jump next time they are in the air
                doubleJumped = false;
            }
            // Is the player is in the air and they are able to double jump?
            else if (canDoubleJump)
            {
                // Smaller jump
                tempVelocity = jumpVelocity * 0.8f;
                rb.velocity = new Vector2(rb.velocity.x, tempVelocity);

                // Can no longer double jump until grounded
                canDoubleJump = false;
                doubleJumped = true;
            }
        }
        else if(!IsGrounded())
        {
            // If the player hasn't double jumped (reset by being grounded)
            if (!doubleJumped)
            {
                canDoubleJump = true;
            }
            // Player is affected by gravity when not dashing
            if (!isDashing)
            {
                rb.velocity = new Vector3(rb.velocity.x, tempVelocity);
            }
            // If the player is dashing, they will be suspended in the air for however long their dash duration is
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, 0);
            }
            if(IsGrounded())
            {
                rb.velocity = new Vector3(rb.velocity.x, 0);

                canDoubleJump = false;
            }
            else if (tempVelocity >= terminalVelocity)
            {
                tempVelocity += gravity;
            }
        }
    }

    public void TrampolineBounce(float bouncePower)
    {
        tempVelocity = jumpVelocity * 1.5f;
        rb.velocity = new Vector3(rb.velocity.x, tempVelocity);
    }

    // on collision enter/exit might have to be moved to the moving platform script
    void OnCollisionEnter2D(Collision2D col)
    {
        // check if moving platform tag
            // parent player to moving platform
    }

    void OnCollisionExit2D(Collision2D col)
    {
        // check if moving platform tag
            // unparent player from moving platform
    }

    // Makes BoxCast visible in scene view
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }
}
