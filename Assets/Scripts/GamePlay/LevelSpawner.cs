using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField] private BrickPool _pool;

    // [Header("Layout")]
    // [SerializeField] private Vector2 _startPos = new Vector2(-6f, 3.5f);
    // [SerializeField] private int columbs = 12;
    // [SerializeField] private int rows = 6;
    // [SerializeField] private Vector2 _spacing = new Vector2(1.1f, 0.55f);

    // [Header("Brick Stats")]
    //  [SerializeField] private int _basePoints = 10;
    // [SerializeField] private bool _harderTopRows = true;
    private readonly List<Brick> _active = new List<Brick>();

    public void Spawn(LevelData data)
    {
        if (data == null) return;

        Clear();

        for (int r = 0; r < data.rows; r++)
        {
            for (int c = 0; c < data.columbs; c++)
            {
                Brick brick = _pool.Get();

                brick.transform.position = new Vector3(data._startPos.x + c * data._spacing.x, data._startPos.y - r * data._spacing.y, 0f);
                //brick.transform.rotation = Quaternion.identity;

                int hp = Mathf.Clamp(data.maxRowHp - r, 1, data.maxRowHp);
                // int _hp = _harderTopRows ? Mathf.Clamp(rows - r, 1, 3) : 1; //üst sıra daha dayanıklı
                int points = hp * data.basePointPerHp;

                brick.Init(this, hpOverride: hp, pointsOverride: points);

                _active.Add(brick);
            }
        }
    }

    public void Despawn(Brick brick)
    {
        _active.Remove(brick);
        _pool.Return(brick);

        //Hepsi bitti mi?
        if (_active.Count == 0)
        {
            GameManager.Instance.OnLevelCleared();
        }
    }

    public void Clear()
    {
        for (int i = _active.Count - 1; i >= 0; i--)
        {
            _pool.Return(_active[i]);
        }
        _active.Clear();
    }
    public int ActiveCount => _active.Count;
}
