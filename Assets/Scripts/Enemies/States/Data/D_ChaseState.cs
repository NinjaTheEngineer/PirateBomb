﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChaseStateData", menuName = "Data/State Data/Chase State")]
public class D_ChaseState : ScriptableObject
{
    public float chaseSpeed = 3f;

    public float chaseTime = 5f;

    public float jumpSpeed = 2f;

    public Vector2 jumpAngle; 
}
