using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected Core core;

    protected GameObject visualGameObject;

    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<Core>();
        visualGameObject = core.GetVisualGO();

        if(core == null) { Debug.LogError("There is no Core on the parent"); }
    }
}
