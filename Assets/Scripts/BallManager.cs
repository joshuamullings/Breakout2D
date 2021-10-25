using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    #region Singleton

        public static BallManager Instance => _instance;
        
        private static BallManager _instance;

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

    public List<BallController> Balls { get; set; }
    public float InitialBallSpeed = 250f;

    [SerializeField] BallController _ballPrefab;
    private BallController _initialBall;
    private Rigidbody2D _initialBallRidgidbody2D;
    
    private void Start()
    {
        InitilizeBall();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStarted)
        {
            // sick ball to player
            Vector3 playerPosition = PlayerController.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(playerPosition.x, playerPosition.y + 0.25f, 0.0f);
            _initialBall.transform.position = ballPosition;

            if (Input.GetMouseButton(0))
            {
                _initialBallRidgidbody2D.isKinematic = false;
                _initialBallRidgidbody2D.AddForce(new Vector2(0.0f, InitialBallSpeed));
                GameManager.Instance.IsGameStarted = true;
            }
        }
    }
    
    private void InitilizeBall()
    {
        Vector3 startingPosition = new Vector3(
            PlayerController.Instance.transform.position.x,
            PlayerController.Instance.transform.position.y + 0.25f);

        _initialBall = Instantiate(_ballPrefab, startingPosition, Quaternion.identity);
        _initialBallRidgidbody2D = _initialBall.GetComponent<Rigidbody2D>();

        this.Balls = new List<BallController>
        {
            _initialBall
        };
    }
}