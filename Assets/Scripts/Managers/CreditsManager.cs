using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayCreditsMusic();
    }
}
