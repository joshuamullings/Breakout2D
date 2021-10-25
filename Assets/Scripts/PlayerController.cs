using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singleton

        public static PlayerController Instance => _instance;
        
        private static PlayerController _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

    #endregion

    public Vector2 DefaultClamp; // x is left wall, y is right wall

    private Camera _camera;
    private SpriteRenderer _spriteRenderer;
    private float _initialPositionY;
    private float _initialSpriteWidthX;

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialPositionY = this.transform.position.y;
        _initialSpriteWidthX = _spriteRenderer.size.x;
    }

    private void Update()
    {
        Vector2 currentClamps = CalculateCurrentClamps();
        this.transform.position = CalculateNewPosition(currentClamps);
    }

    private Vector2 CalculateCurrentClamps()
    {
        float shift = 0.54f * DefaultClamp.x * (_spriteRenderer.size.x / _initialSpriteWidthX - 1);
        return new Vector2(DefaultClamp.x + shift, DefaultClamp.y - shift);
    }

    private Vector3 CalculateNewPosition(Vector2 currentClamps)
    {
        float mousePositionPixelsX = Mathf.Clamp(Input.mousePosition.x, currentClamps.x, currentClamps.y);
        float mousePositionWorldX = _camera.ScreenToWorldPoint(new Vector3(mousePositionPixelsX, 0.0f, 0.0f)).x;
        return new Vector3(mousePositionWorldX, _initialPositionY, 0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRidgidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 centre = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);
            ballRidgidbody2D.velocity = Vector2.zero; // zero velocity before adding force later
            float difference = centre.x - hitPoint.x;
            float bounceForceX = Mathf.Abs(difference * 200);

            if (hitPoint.x < centre.x)
            {
                ballRidgidbody2D.AddForce(new Vector2(-bounceForceX, BallManager.Instance.InitialBallSpeed));
            }
            else
            {
                ballRidgidbody2D.AddForce(new Vector2(bounceForceX, BallManager.Instance.InitialBallSpeed));
            }
        }
    }
}