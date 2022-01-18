using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Singleton

        public static Player Instance => _instance;
        
        private static Player _instance;

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

    public GameObject ShadowGameObject;
    public Vector2 DefaultClamp; // x is left wall, y is right wall
    public float BallHitModifier = 1500.0f;
    public float ExtendedShrinkDuration = 10.0f;
    public float PlayerWidth = 1.0f;
    public float PlayerHeight = 0.25f;

    private Camera _camera;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _spriteRendererShadow;
    private BoxCollider2D _boxCollider2D;
    private float _initialPositionY;
    private float _initialSpriteWidthX;
    private float _previousX = 0.0f;
    private float _ballHitModifierFactor;
    
    public bool PlayerIsTransforming { get; set; }

    public void StartWidthAnimation(float newWidth)
    {
        StartCoroutine(AnimatePlayerWidth(newWidth));
    }

    public IEnumerator AnimatePlayerWidth(float width)
    {
        PlayerIsTransforming = true;
        StartCoroutine(ResetPlayerWidthAfterTime(ExtendedShrinkDuration));

        if (width > _spriteRenderer.size.x)
        {
            float currentWidth = _spriteRenderer.size.x;

            while (currentWidth < width)
            {
                currentWidth += Time.deltaTime * 2.0f;
                _spriteRenderer.size = new Vector2(currentWidth, PlayerHeight);
                _spriteRendererShadow.size = new Vector2(currentWidth, PlayerHeight);
                _boxCollider2D.size = new Vector2(currentWidth, PlayerHeight);
                yield return null;
            }
        }
        else
        {
            float currentWidth = _spriteRenderer.size.x;

            while (currentWidth > width)
            {
                currentWidth -= Time.deltaTime * 2.0f;
                _spriteRenderer.size = new Vector2(currentWidth, PlayerHeight);
                _spriteRendererShadow.size = new Vector2(currentWidth, PlayerHeight);
                _boxCollider2D.size = new Vector2(currentWidth, PlayerHeight);
                yield return null;
            }
        }

        PlayerIsTransforming = false;
    }

    private IEnumerator ResetPlayerWidthAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartWidthAnimation(PlayerWidth);
    }

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRendererShadow = ShadowGameObject.GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _initialPositionY = this.transform.position.y;
        _initialSpriteWidthX = _spriteRenderer.size.x;
    }

    private void Update()
    {
        Vector2 currentClamps = CalculateCurrentClamps();
        this.transform.position = CalculateNewPosition(currentClamps);

        float currentX = transform.position.x;
        float _playerSpeed = currentX - _previousX;
        _ballHitModifierFactor  = BallHitModifier * _playerSpeed;
        _previousX = currentX;
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
                ballRidgidbody2D.AddForce(new Vector2(-bounceForceX + _ballHitModifierFactor, BallManager.Instance.InitialBallSpeed));
            }
            else
            {
                ballRidgidbody2D.AddForce(new Vector2(bounceForceX + _ballHitModifierFactor, BallManager.Instance.InitialBallSpeed));
            }
        }
    }
}