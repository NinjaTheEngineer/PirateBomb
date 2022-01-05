using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour, IKnockbackable, IDefaultKnockback
{
    public Rigidbody2D rb;
    private Vector2 explosionOrigin;

    public float bombDamage = 1f;
    public float knockbackStrength = 10f;
    public float maxVelocity;
    
    public float timeToDetonate = 2f;
    public float extraDetonateTime = 0.75f;
    public float explosionRadius = 1f;

    public Vector2 defaultKnockbackAngle = Vector2.zero;
    public float xDetonationPowerAmp = 2f;
    public float yDetonationPowerAmp = 3f;

    private Animator anim;
    private Collider2D collider2D;
    private bool exploding = false;
    
    private float sqrMaxVelocity;
    private Vector2 rbVelocity;

    [SerializeField]
    private Vector2 launchAngle = new Vector2(1, 3);



    private void Awake()
    {
        SetMaxVelocity(maxVelocity);
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }
    private void Update()
    {
        timeToDetonate -= Time.deltaTime;
        if (timeToDetonate < 0 && !exploding)
        {
            StartExplosion();
        }
    }
    private void FixedUpdate()
    {
        rbVelocity = rb.velocity;

        if (rbVelocity.sqrMagnitude > sqrMaxVelocity)
        {
            rb.velocity = rbVelocity.normalized * maxVelocity;
        }
    }

    public void SetMaxVelocity(float maxVelocity)
    {
        this.maxVelocity = maxVelocity;
        sqrMaxVelocity = maxVelocity * maxVelocity;
    }
    public void StartExplosion()
    {
        transform.localRotation = Quaternion.identity;
        rb.rotation = 0f;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        exploding = true;
        anim.SetTrigger("explode");
        CameraShake.Instance.ShakeCamera(0.75f, 0.2f, false);
        Detonate();
    }
    public void LaunchBomb(float force, int facingDirection)
    {
        Vector2 rbVelocity = new Vector2(launchAngle.x * force * facingDirection, launchAngle.y * force);
        Debug.Log("[Bomb]: launchAngle.x => " + launchAngle.x);
        Debug.Log("[Bomb]: launchAngle.y => " + launchAngle.y);
        Debug.Log("[Bomb]: Launch Angle => " + rbVelocity);
        rb.velocity = rbVelocity;
    }

    public void Knockback(float knockbackStrength, Vector2 bombPosition)
    {
        Vector2 angle = new Vector2(bombPosition.x - transform.position.x, transform.position.y - bombPosition.y).normalized;
        Vector2 rbVelocity;
        Debug.Log("Angle : " + angle);

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
            Debug.Log("rbVelocity : " + rbVelocity);
            timeToDetonate += extraDetonateTime;
            rb.velocity = rbVelocity;
        }
    }

    public void Knockback(int facingDirection)
    {
        Vector2 rbVelocity;

        if (!rb.bodyType.Equals(2))
        {
            rbVelocity = new Vector2(defaultKnockbackAngle.x * facingDirection, defaultKnockbackAngle.y);
            timeToDetonate += extraDetonateTime;
            rb.velocity = rbVelocity;
        }
    }

    private bool TargetAndBombHaveSameHeight(Vector2 targetPosition, Vector2 bombPosition)
    {
        return bombPosition.y + 1 > targetPosition.y;
    }
    private void Detonate()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(rb.position, explosionRadius);

        foreach (Collider2D collider in detectedObjects)
        {
            if (collider.Equals(collider2D))
                continue;

            IKnockbackable knockbackable = collider.GetComponent<IKnockbackable>() != null ?
                collider.GetComponent<IKnockbackable>() : collider.GetComponentInParent<IKnockbackable>();
            IDamageable damageable = collider.GetComponent<IDamageable>() != null ?
                collider.GetComponent<IDamageable>() : collider.GetComponentInParent<IDamageable>();

            Debug.Log("Collider -> " + collider + " _knockbackable_ -> " + knockbackable);
            if (knockbackable != null)
            {
                explosionOrigin = new Vector2(transform.position.x, transform.position.y - 1);
                knockbackable.Knockback(knockbackStrength, explosionOrigin);
            }

            if(damageable != null)
            {
                damageable.Damage(bombDamage);
            }
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(rb.position, explosionRadius);
        Gizmos.DrawWireSphere(explosionOrigin, 0.25f);
    }
}
