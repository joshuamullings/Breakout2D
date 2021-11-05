using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Ball ball = collision.GetComponent<Ball>();
            BallManager.Instance.Balls.Remove(ball);
            ball.Kill();
        }
    }
}