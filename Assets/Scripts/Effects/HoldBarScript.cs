using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBarScript: MonoBehaviour
{
    public Animator anim;
    private bool isEnabled = false;

    private void Start() //Initialize variables
    {
        anim = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    private void Update() //Update logic, the animations here
    {
        UpdateAnimations();
    }

    private void UpdateAnimations() //Update animations
    {
        anim.SetBool("released", !isEnabled);
    }

    public void BombReleased() //Disable effect when bomb released
    {
        DisableEffect();
    }

    public void EnableEffect() //Enable the bomb bar effect
    {
        if (!isEnabled)
        {
            gameObject.SetActive(true);
            isEnabled = true;
        }
    }
    public void FullHold() //Set the full charged effect
    {
        anim.SetBool("full", true);
    }
    public void DisableEffect() //Disable the bomb bar effect
    {
        if (isEnabled)
        {
            gameObject.SetActive(false);
            isEnabled = false;
        }
    }
}
