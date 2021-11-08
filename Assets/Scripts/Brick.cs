using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    public ParticleSystem HitEffect;
    public ParticleSystem DestroyEffect;
    public int Hitpoints = 3;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;

    public static event Action<Brick> OnBrickDestruction;

    public void Initialize(Transform containerTransform, Sprite sprite, Color colour, int hitpoints)
    {
        transform.SetParent(containerTransform);
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.color = colour;
        Hitpoints = hitpoints;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        Ball.OnLighteningBallEnable += OnLighteningBallEnable;
        Ball.OnLighteningBallDisable += OnLighteningBallDisable;
    }

    private void OnLighteningBallEnable(Ball obj)
    {
        if (this != null)
        {
            _boxCollider2D.isTrigger = true;
        }
    }

    private void OnLighteningBallDisable(Ball obj)
    {
        if (this != null)
        {
            _boxCollider2D.isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Ball ball = collider.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        Hitpoints--;

        if (Hitpoints <= 0 || (ball != null && ball.IsLighteningBall))
        {
            BrickManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            OnBrickDestroy();
            SpawnEffect(DestroyEffect, true);
            Destroy(this.gameObject);
        }
        else
        {
            SpawnEffect(HitEffect, false);
            _spriteRenderer.sprite = BrickManager.Instance.Sprites[Hitpoints - 1];
        }
    }

    private void OnBrickDestroy()
    {
        float buffSpawnChance = UnityEngine.Random.Range(0, 100.0f);
        float debuffSpawnChance = UnityEngine.Random.Range(0, 100.0f);
        bool alreadySpawned = false;

        if (buffSpawnChance <= BuffManager.Instance.BuffChance)
        {
            alreadySpawned = true;
            Buff newBuff = SpawnBuff(true);
        }

        if (debuffSpawnChance <= BuffManager.Instance.DebuffChance && !alreadySpawned)
        {
            Buff newDebuff = SpawnBuff(false);
        }
    }

    private Buff SpawnBuff(bool isBuff)
    {
        List<Buff> buff;

        if (isBuff)
        {
            buff = BuffManager.Instance.AvalibleBuffs;
        }
        else
        {
            buff = BuffManager.Instance.AvalibleDebuffs;
        }

        int buffIndex = UnityEngine.Random.Range(0, buff.Count);
        Buff prefab = buff[buffIndex];
        Buff newBuff = Instantiate(prefab, transform.position, Quaternion.identity);

        return newBuff;
    }

    private void SpawnEffect(ParticleSystem particleEffect, bool colour)
    {
        Vector3 brickPosition = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPosition.x, brickPosition.y, brickPosition.z - 0.2f);
        GameObject effect = Instantiate(particleEffect.gameObject, spawnPosition, Quaternion.identity);
        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        if (colour) mm.startColor = _spriteRenderer.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    private void OnDisable()
    {
        Ball.OnLighteningBallEnable -= OnLighteningBallEnable;
        Ball.OnLighteningBallDisable -= OnLighteningBallDisable;
    }
}