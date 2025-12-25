using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPool : MonoBehaviour
{
    [SerializeField] private Brick _brickPrefab;
    [SerializeField] private int _prewarmCount = 80;

    private readonly Queue<Brick> _pool = new Queue<Brick>();

    private void Awake()
    {
        for (int i = 0; i < _prewarmCount; i++)
        {
            Brick b = Instantiate(_brickPrefab, transform);
            b.gameObject.SetActive(false);
            _pool.Enqueue(b);
        }
    }
    public Brick Get()
    {
        if(_pool.Count > 0)
        {
            Brick b = _pool.Dequeue();
            b.gameObject.SetActive(true);
            return b;
        }

        // Yetmezse genişletiyoruz
        Brick nb = Instantiate(_brickPrefab, transform);
        nb.gameObject.SetActive(true);
        return nb;
    }

    public void Return(Brick brick)
    {
        brick.gameObject.SetActive(false);
        brick.transform.SetParent(transform);
        _pool.Enqueue(brick);
    }
}
