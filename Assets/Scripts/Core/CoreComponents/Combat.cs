using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent
{
    public bool isKnockbackActive = false;
    private float knockbackStartTime;
    private float knockbackDuration = 0.4f;

    public float xDetonationPowerAmp = 1f;
    public float yDetonationPowerAmp = 1f;
    Vector2 rbVelocity;
    public void SetKnockbackActive(bool isKnockbackActive)
    {
        this.isKnockbackActive = isKnockbackActive;
    }

    public void LogicUpdate()
    {
        CheckKnockback();
    }
    public void Knockback(float knockbackStrength, Vector2 bombPosition)
    {
        Vector2 visualGO = core.visualGO.transform.position;
        Vector2 angle = new Vector2(visualGO.x - bombPosition.x, visualGO.y - bombPosition.y).normalized;

        if (!core.Movement.rb.bodyType.Equals(2))
        {
            int direction = transform.position.x > bombPosition.x ? 1 : -1;

            Debug.Log("transform.position.x > bombPosition.x: " + (visualGO.x > bombPosition.x) + " _ transform.position.x - " + visualGO.x + " _bombPosition.x - " + bombPosition.x);
            if (TargetAndBombHaveSameHeight(visualGO, bombPosition))
            {
                rbVelocity = new Vector2(angle.x * direction * knockbackStrength * xDetonationPowerAmp,
                    angle.y * knockbackStrength * yDetonationPowerAmp);
            }
            else
            {
                rbVelocity = new Vector2(angle.x * direction * knockbackStrength * xDetonationPowerAmp,
                    angle.y * knockbackStrength );
            }
        }
        core.Movement.rb.isKinematic = true;
        ApplyKnockback();
        core.Movement.rb.isKinematic = false;
    }

    public void ApplyKnockback()
    {
        Debug.Log("KNOCKBACK Angle : " + rbVelocity);
        core.Movement.SetVelocityZero();
        knockbackStartTime = Time.time;
        isKnockbackActive = true;
        core.Movement.SetVelocity(rbVelocity);
    }
    private bool TargetAndBombHaveSameHeight(Vector2 targetPosition, Vector2 bombPosition)
    {
        return bombPosition.y + 1 > targetPosition.y;
    }

    private void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && isKnockbackActive && core.CollisionSenses.Ground)
        {
            isKnockbackActive = false;
            core.Movement.CanSetVelocity = true;
            core.Movement.SetVelocity(new Vector2(0f, core.Movement.rb.position.y));
        }
    }
}