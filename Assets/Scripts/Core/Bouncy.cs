using UnityEngine;

public class Bouncy : MonoBehaviour
{
    [SerializeField] private float _extraSpeed = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball _ball = collision.gameObject.GetComponent<Ball>();

        if(_ball == null) return;
        {
            _ball.BoostSpeed(_extraSpeed);
        }
    } 
}
