using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D rb { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }

    public bool CanSetVelocity { get; set; }

    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();
        CanSetVelocity = true;
        rb = visualGameObject.GetComponent<Rigidbody2D>();
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
    private void SetFinalVelocity()
    {
        if (CanSetVelocity)
        {
            rb.velocity = workspace;
            CurrentVelocity = workspace;
        }
    }

    #endregion
}
