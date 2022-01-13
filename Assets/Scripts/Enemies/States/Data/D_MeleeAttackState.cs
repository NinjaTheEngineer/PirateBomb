using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackState", menuName = "Data/State Data/Melee AttackState")]
public class D_MeleeAttackState : ScriptableObject
{
    public int attackDamage = 1;
    public float attackRadius = 0.5f;

    public float knockbackStrength = 1f;

    public LayerMask whatIsPlayer;
    public LayerMask whatIsBomb;
}
