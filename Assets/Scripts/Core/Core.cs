using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject visualGO;

    public Movement Movement { get; private set; }

    private void Awake()
    {
        Movement = GetComponentInChildren<Movement>();

        if (!Movement)
        {
            Debug.LogError("Missing Core Component");
        }
    }

    public void LogicUpdate()
    {
        Movement.LogicUpdate();
    }

    public GameObject GetVisualGO()
    {
        return visualGO != null ? visualGO : null;
    }

}

