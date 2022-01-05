using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    #region Check Transforms

    public Transform GroundCheck { get; private set; }
    public Transform WallCheck { get; private set; }
    public Transform FloorLedgeCheck { get; private set; }
    public Transform UpperLedgeCheck { get; private set; }
    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }


    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform upperLedgeCheck;
    [SerializeField] private Transform floorLedgeCheck;

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float upperLedgeCheckDistance;
    [SerializeField] private float floorLedgeCheckDistance;

    [SerializeField] private LayerMask whatIsGround;

    #endregion

    public bool Ground
    {
        get => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * core.Movement.FacingDirection, wallCheckDistance, whatIsGround);
    }
    public bool WallBeforePlayer(float distance)
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * core.Movement.FacingDirection, distance, whatIsGround);
    }
    public bool UpperLedge
    {
        get => (!Physics2D.Raycast(upperLedgeCheck.position, transform.right * core.Movement.FacingDirection, upperLedgeCheckDistance, whatIsGround));
    }
    public bool CanJumpLedge
    {
        get => (!Physics2D.Raycast(upperLedgeCheck.position, transform.right * core.Movement.FacingDirection, upperLedgeCheckDistance, whatIsGround) &&
            WallFront && Ground);
    }

    public bool FloorLedge
    {
        get => (!Physics2D.Raycast(floorLedgeCheck.position, Vector2.down, floorLedgeCheckDistance, whatIsGround) &&
            Ground);
    }

}
