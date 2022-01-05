using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject : MonoBehaviour, IKnockbackable
{
    public float weight;

    public float xKnockbackMultiplier;
    public float yKnockbackMultiplier;

    public Rigidbody2D rb { get; private set; }
    private Vector2 rbVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Knockback(float knockbackStrength, Vector2 knockbackPosition)
    {
        Vector2 position = transform.position;
        Vector2 angle = new Vector2(position.x - knockbackPosition.x, position.y - knockbackPosition.y).normalized;

        if (!rb.bodyType.Equals(2))
        {

            Debug.Log("transform.position.x > bombPosition.x: " + (position.x > knockbackPosition.x) + " _ transform.position.x - " + position.x + " _bombPosition.x - " + knockbackPosition.x);
            if (TargetAndBombHaveSameHeight(position, knockbackPosition))
            {
                rbVelocity = new Vector2(angle.x * knockbackStrength * xKnockbackMultiplier,
                    angle.y * knockbackStrength * yKnockbackMultiplier);
            }
            else
            {
                rbVelocity = new Vector2(angle.x * knockbackStrength * xKnockbackMultiplier,
                    angle.y * knockbackStrength);
            }

            rb.velocity = rbVelocity;
        }
    }

    private bool TargetAndBombHaveSameHeight(Vector2 targetPosition, Vector2 originPosition)
    {
        return originPosition.y + 1 > targetPosition.y;
    }
}
