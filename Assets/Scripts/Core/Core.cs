using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject visualGO;
    public Movement Movement { get; private set; }
    public Combat Combat { get; private set; }
    public CollisionSenses CollisionSenses { get; private set; }

    private void Awake() //Initialize components
    {
        Movement = GetComponentInChildren<Movement>();
        Combat = GetComponentInChildren<Combat>();
        CollisionSenses = GetComponentInChildren<CollisionSenses>();

        if (!Movement || !Combat)
        {
            Debug.LogError("Missing Core Component");
        }
    }

    public void LogicUpdate() //Logic update, same has an Update of Monobehaviour
    {
        Movement.LogicUpdate();
        Combat.LogicUpdate();
    }

    public GameObject GetVisualGO()
    {
        return visualGO != null ? visualGO : null;
    }

}

