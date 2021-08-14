using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float wallCheckDistance = 0.2f;
    public float groundCheckDistance = 0.4f;

    public float upperLedgeCheckDistance = 0.4f;
    public float floorLedgeCheckDistance = 0.4f;

    public float playerJumpAboveDistance = 2f;

    public float minAgroDistance = 3f;
    public float maxAgroDistance = 4f;

    public float closeRangeActionDistance = 1f;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

}
