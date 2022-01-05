using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D rb { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workspace;

    public int FacingDirection { get; private set; }
    public bool CanSetVelocity { get; set; }


    protected override void Awake()
    {
        base.Awake();
        FacingDirection = 1;
        CanSetVelocity = true;
        rb = GetComponentInParent<Rigidbody2D>();
    }

    public void LogicUpdate()
    {
        CurrentVelocity = rb.velocity;
    }

    #region Set Functions

    public void SetVelocityZero()
    {
        rb.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }
    public void SetVelocity(Vector2 velocity)
    {
        workspace.Set((float)velocity.x, (float)velocity.y);
        SetFinalVelocity();
    }
    public void SetVelocity(float velocity, int direction)
    {
        workspace.Set((float)direction * velocity, rb.velocity.y);
        rb.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        SetFinalVelocity();
    }
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        SetFinalVelocity();
    }
    private void SetFinalVelocity()
    {
        if (CanSetVelocity)
        {
            rb.velocity = workspace;
            CurrentVelocity = workspace;
        }
    }
    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    public void Flip()
    {
        FacingDirection *= -1;
        visualGameObject.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    #endregion
}