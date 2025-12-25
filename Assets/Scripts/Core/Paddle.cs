using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float _speed = 10.0f;
    public bool _isPlayer;
    protected Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void ResetPaddlePosition()
    {
        _rb.position = new Vector2(0.0f, _rb.position.y);
        _rb.linearVelocity = Vector2.zero;
    }
}
