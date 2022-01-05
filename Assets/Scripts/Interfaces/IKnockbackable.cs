using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackable
{
    void Knockback(float knockbackStrength, Vector2 knockbackPosition);
}