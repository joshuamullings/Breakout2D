using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public ParticleSystem LightningEffect;
    public GameObject LightningLightEffect;
    public bool IsLightningBall;
    
    private SpriteRenderer[] _spriteRenderers;
    private float _lighteningBallDuration = 5.0f;
    
    public static event Action<Ball> OnBallKill;
    public static event Action<Ball> OnLighteningBallEnable;
    public static event Action<Ball> OnLighteningBallDisable;

    public void Kill()
    {
        OnBallKill?.Invoke(this);
        Destroy(gameObject, 1);
    }

    public void StartLighteningBall()
    {
        if (!IsLightningBall)
        {
            foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
            {
                spriteRenderer.enabled = true;
            }

            IsLightningBall = true;
            LightningEffect.gameObject.SetActive(true);
            LightningLightEffect.SetActive(true);
            StartCoroutine(StopLighteningBallAfterTime(_lighteningBallDuration));
            OnLighteningBallEnable?.Invoke(this);
        }
    }

    private IEnumerator StopLighteningBallAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StopLighteningBall();
    }

    private void StopLighteningBall()
    {
        if (IsLightningBall)
        {
            foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
            {
                spriteRenderer.enabled = true;
            }

            IsLightningBall = false;
            LightningEffect.gameObject.SetActive(false);
            LightningLightEffect.SetActive(false);
            OnLighteningBallDisable?.Invoke(this);
        }
    }

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }
}