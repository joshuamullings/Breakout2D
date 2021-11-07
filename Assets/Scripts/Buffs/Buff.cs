using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    protected abstract void ApplyEffect();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            ApplyEffect();
        }

        if (
            collider.gameObject.CompareTag("Player") ||
            collider.gameObject.CompareTag("KillWall")
         ) {
             Destroy(gameObject);
        }
    }
}