using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBarScript: MonoBehaviour
{
    public Animator anim;
    private bool isEnabled = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        anim.SetBool("released", !isEnabled);
    }

    public void BombReleased()
    {
        DisableEffect();
    }

    public void EnableEffect()
    {
        if (!isEnabled)
        {
            gameObject.SetActive(true);
            isEnabled = true;
        }
    }
    public void FullHold()
    {
        anim.SetBool("full", true);
    }
    public void DisableEffect()
    {
        if (isEnabled)
        {
            gameObject.SetActive(false);
            isEnabled = false;
        }
    }
}
