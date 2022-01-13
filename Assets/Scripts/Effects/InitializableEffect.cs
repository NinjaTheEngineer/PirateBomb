using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializableEffect : MonoBehaviour
{
    public void ActivateEffect() //Activates any effect
    {
        gameObject.SetActive(true);
    }
    public void DeactivateEffect() //Deactivates any effect
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
