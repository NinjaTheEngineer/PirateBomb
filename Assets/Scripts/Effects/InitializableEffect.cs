using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializableEffect : MonoBehaviour
{
    public void ActivateEffect()
    {
        gameObject.SetActive(true);
    }
    public void DeactivateEffect()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
