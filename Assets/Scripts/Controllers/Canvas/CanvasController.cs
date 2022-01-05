using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public void SlowTime()
    {
        Time.timeScale -= 0.1f;
    }

    public void FastenTime()
    {
        Time.timeScale += 0.1f;
    }
}
