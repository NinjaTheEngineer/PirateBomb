using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAnimationToStateMachine : MonoBehaviour
{
    private void Destroy()
    {
        GetComponentInParent<BombController>().Destroy();
    }
}
