using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float LeftClamp;
    public float RightClamp;

    private Camera _camera;
    private float _initialPositionY;

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
        _initialPositionY = this.transform.position.y;
    }

    private void Update()
    {
        float mousePositionPixelsX = Mathf.Clamp(Input.mousePosition.x, LeftClamp, RightClamp);
        float mousePositionWorldX = _camera.ScreenToWorldPoint(new Vector3(mousePositionPixelsX, 0.0f, 0.0f)).x;
        this.transform.position = new Vector3(mousePositionWorldX, _initialPositionY, 0.0f);
    }
}
