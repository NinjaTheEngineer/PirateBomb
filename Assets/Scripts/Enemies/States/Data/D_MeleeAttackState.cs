using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackState", menuName = "Data/State Data/Melee AttackState")]
public class D_MeleeAttackState : ScriptableObject
{
    public float attackDamage = 10f;
    public float attackRadius = 0.5f;
    public LayerMask whatIsPlayer;
}
