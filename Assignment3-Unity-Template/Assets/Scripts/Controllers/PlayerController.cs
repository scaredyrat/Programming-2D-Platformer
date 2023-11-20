using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection { Left, Right }

    public bool IsWalking()
    {
        throw new System.NotImplementedException( "IsWalking in PlayerController has not been implemented." );
    }

    public bool IsGrounded()
    {
        throw new System.NotImplementedException( "IsGrounded in PlayerController has not been implemented." );
    }

    public FacingDirection GetFacingDirection()
    {
        throw new System.NotImplementedException( "GetFacingDirection in PlayerController has not been implemented." );
    }
}
