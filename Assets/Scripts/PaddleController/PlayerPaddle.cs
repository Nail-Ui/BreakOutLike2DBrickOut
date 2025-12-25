using UnityEngine;

public class PlayerPaddle : Paddle
{
    private Vector2 _direction;
    private float _inputX;
    private void Update()
    {
        if (_isPlayer)
        {
            _inputX = Input.GetAxisRaw("Horizontal");
        }

        _direction = new Vector2(_inputX, 0);
    }

    private void FixedUpdate()
    {
        if(_direction.sqrMagnitude != 0)
        {
            _rb.AddForce(_direction * _speed);
        }
    }
}
