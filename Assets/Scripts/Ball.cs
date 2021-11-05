using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static event Action<Ball> OnBallKill;

    public void Kill()
    {
        OnBallKill?.Invoke(this);
        Destroy(gameObject, 1);
    }
}