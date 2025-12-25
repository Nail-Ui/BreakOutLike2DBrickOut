using UnityEngine;

public class BottomGoalZone : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Ball")) return;
        {
            _gameManager.OnBallLoss();
        }
    } 
}
