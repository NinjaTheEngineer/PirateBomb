using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChargeStateData", menuName = "Data/State Data/Charge State")]
public class D_ChargeState : ScriptableObject
{
    public float chargeSpeed = 3f;

    public float chargeTime = 2f;
}
