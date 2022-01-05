using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockbackable : MonoBehaviour, IKnockbackable
{
    public float xDetonationPowerAmp = 1f;
    public float yDetonationPowerAmp = 1f;

    public Rigidbody2D rb;

    public void Knockback(float knockbackStrength, Vector2 bombPosition)
    {
        Vector2 angle = new Vector2(bombPosition.x - transform.position.x, bombPosition.y - transform.position.y).normalized;
        Vector2 rbVelocity;

        if (!rb.bodyType.Equals(2))
        {
            if (TargetAndBombHaveSameHeight(transform.position, bombPosition))
            {
                rbVelocity = new Vector2(angle.x * -1 * knockbackStrength * xDetonationPowerAmp,
                    angle.y * knockbackStrength * yDetonationPowerAmp);
            }
            else
            {
                rbVelocity = new Vector2(angle.x * -1 * knockbackStrength * xDetonationPowerAmp,
                    angle.y * knockbackStrength * yDetonationPowerAmp);
            }
            Debug.Log("Angle : " + rbVelocity);

            rb.velocity = rbVelocity;
        }
    }

    public void Knockback(float knockbackStrength, int facingDirection)
    {
        throw new System.NotImplementedException();
    }

    private bool TargetAndBombHaveSameHeight(Vector2 targetPosition, Vector2 bombPosition)
    {
        return bombPosition.y + 1 > targetPosition.y;
    }
}
