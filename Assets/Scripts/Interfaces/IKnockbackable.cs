using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockbackable //For any object thaht receives knockback from another object
{
    void Knockback(float knockbackStrength, Vector2 knockbackPosition);
}