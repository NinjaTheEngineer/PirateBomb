using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected D_MeleeAttackState stateData;
    protected bool playerJumpedAboveWhileAttacking;
    protected bool playerIsDead;

    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        if (playerJumpedAbove)
        {
            playerJumpedAboveWhileAttacking = true;
        }
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();

        if (playerJumpedAboveWhileAttacking)
        {
            entity.Flip();
            playerJumpedAboveWhileAttacking = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedPlayer = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);
        Collider2D[] detectedBomb = Physics2D.OverlapCircleAll(attackPosition.position, stateData.attackRadius, stateData.whatIsBomb);
        
        foreach (Collider2D collider in detectedPlayer)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(stateData.attackDamage);
            }

            //IKnockbackable knockbackable = collider.GetComponent<IKnockbackable>();
            IDefaultKnockback knockbackable = collider.GetComponent<IDefaultKnockback>();

            if (knockbackable != null)
            {
                //knockbackable.Knockback(stateData.knockbackStrength, attackPosition.position);
                knockbackable.Knockback(core.Movement.FacingDirection);
            }
        }

        foreach(Collider2D collider in detectedBomb)
        {
            Debug.Log("Bomb detected!");
            IDefaultKnockback knockbackable = collider.GetComponent<IDefaultKnockback>();

            if (knockbackable != null)
            {
                Debug.Log("Knockbar Bomb");
                knockbackable.Knockback(core.Movement.FacingDirection);
            }
        }
    }
}
