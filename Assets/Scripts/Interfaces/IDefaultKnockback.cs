using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDefaultKnockback //For any object that receives knockback
{
    void Knockback(int facingDirection);
}
