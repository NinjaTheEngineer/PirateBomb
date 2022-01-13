using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjects : MonoBehaviour
{
    private EnvironmentObject parentObject;
    private void Start()
    {
        parentObject = GetComponentInParent<EnvironmentObject>();
    }
    private void OnTriggerEnter2D(Collider2D collision) //Push objects by enemies player or bombs
    {
        GameObject collider = collision.gameObject;
        if (collider.CompareTag("Enemy") || collider.CompareTag("Player") || collider.CompareTag("Bomb"))
        {
            Rigidbody2D colliderRb = collider.GetComponent<Rigidbody2D>();
            if(colliderRb != null)
            {
                Vector2 pushVelocity = colliderRb.velocity / parentObject.weight;
                if (pushVelocity.y <= 0.01f && pushVelocity.y >= -0.01f)
                    pushVelocity.y = 0.75f;
                Debug.Log("pushVelocity - " + pushVelocity);
                parentObject.rb.velocity = pushVelocity;
            }
        }
    }
}
