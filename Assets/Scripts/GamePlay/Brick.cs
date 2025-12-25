using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private int _hitPoints = 1;
    [SerializeField] private int _points = 10;

    private int _hp;
    private LevelSpawner _owner;

    public void Init(LevelSpawner owner, int hpOverride = -1, int pointsOverride = -1)
    {
        _owner = owner;
        _hp = (hpOverride > 0) ? hpOverride : _hitPoints;
        if(pointsOverride > 0) _points = pointsOverride;
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.collider.CompareTag("Ball")) return;

        _hp--;

        if(_hp <= 0)
        {
            //score + SFX
            GameManager.Instance.AddScore(_points);
            AudioManager.Instance.PlaySfx(GameAudioLibrary.Instance._brickBreak);

            //Pool/Spawner'a geri gönder
            _owner.Despawn(this);
        }
        else
        {
            AudioManager.Instance.PlaySfx(GameAudioLibrary.Instance._brickBreak);
        }
    }


}
