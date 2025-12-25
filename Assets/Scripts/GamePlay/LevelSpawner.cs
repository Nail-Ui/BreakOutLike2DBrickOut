using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField] private BrickPool _pool;

    [Header("Layout")]
    [SerializeField] private Vector2 _startPos = new Vector2(-6f, 3.5f);
    [SerializeField] private int columbs = 12;
    [SerializeField] private int rows = 6;
    [SerializeField] private Vector2 _spacing = new Vector2(1.1f, 0.55f);

    [Header("Brick Stats")]
    [SerializeField] private int _basePoints = 10;
    [SerializeField] private bool _harderTopRows = true;

    private readonly List<Brick> _active = new List<Brick>();

    public int ActiveCount => _active.Count;

    public void SpawnLevel()
    {
        ClearLevel();

        for(int r = 0; r < rows; r++)
        {
            for(int c = 0; c < columbs; c++)
            {
                Brick brick = _pool.Get();
                brick.transform.position = new Vector3(_startPos.x + c * _spacing.x, _startPos.y - r * _spacing.y, 0f);
                brick.transform.rotation = Quaternion.identity;

                int _hp = _harderTopRows ? Mathf.Clamp(rows - r, 1, 3) : 1; //üst sıra daha dayanıklı
                int pts = _basePoints * _hp;

                brick.Init(this, hpOverride: _hp, pointsOverride: pts);

                _active.Add(brick);
            }
        }
    }

    public void Despawn(Brick brick)
    {
        _active.Remove(brick);
        _pool.Return(brick);

        //Hepsi bitti mi?
        if(_active.Count == 0)
        {
            GameManager.Instance.PaddleAndBallPosReset();
        }
    }

    public void ClearLevel()
    {
        for(int i = _active.Count - 1; i >= 0; i--)
        {
            _pool.Return(_active[i]);
        }
        _active.Clear();
    }

}
