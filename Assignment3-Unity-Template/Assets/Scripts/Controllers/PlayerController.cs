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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = FacingDirection.Right;
    }

    public bool IsWalking()
    {
        float inputX = PlayerInput.GetDirectionalInput().x;
        rb.velocity = new Vector2(speed * inputX, 0);

        if (inputX != 0)
        {
            return true;
        }
        return false;
    }

    public bool IsGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        return false;
    }

    public FacingDirection GetFacingDirection()
    {
        float inputX = PlayerInput.GetDirectionalInput().x;
        if(inputX > 0)
        {
            return direction = FacingDirection.Right;
        }
        else if(inputX < 0)
        {
            return direction = FacingDirection.Left;
        }
        return direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position-transform.up * castDistance, boxSize);
    }
}
