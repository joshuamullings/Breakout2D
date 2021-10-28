using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    public static event Action<Brick> OnBrickDestruction;
    public ParticleSystem DestroyEffect;
    public int Hitpoints = 3;

    private SpriteRenderer _spriteRenderer;

    public void Init(Transform containerTransform, Sprite sprite, Color colour, int hitpoints)
    {
        transform.SetParent(containerTransform);
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.color = colour;
        Hitpoints = hitpoints;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Brick brick = collision.gameObject.GetComponent<Brick>();
        ApplyCollisionLogic(brick);

    }

    private void ApplyCollisionLogic(Brick brick)
    {
        Hitpoints--;

        if (Hitpoints <= 0)
        {
            OnBrickDestruction?.Invoke(this);
            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            _spriteRenderer.sprite = BrickManager.Instance.Sprites[Hitpoints - 1];
        }
    }

    private void SpawnDestroyEffect()
    {
        Vector3 brickPosition = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPosition.x, brickPosition.y, brickPosition.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);
        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = _spriteRenderer.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }
}