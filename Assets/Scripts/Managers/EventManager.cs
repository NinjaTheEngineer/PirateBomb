using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;

    public void BroadCastPlayerDeath() //Tells every enemy that the player is dead
    {
        if (OnPlayerDeath != null)
        {
            OnPlayerDeath();
        }
    }
}
