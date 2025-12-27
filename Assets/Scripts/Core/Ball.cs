using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // [Header("Paddle Bounce Settings")]
    // [SerializeField] private float _paddleHitStrength = 1.2f;
    // [SerializeField] private float _paddleXVelocityMultiplier = 8f;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 100f;

    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private Transform _paddle;

    [SerializeField] private float _offsetY = 0.5f;
    // private bool _isActive;
    public bool _isAttached = true;
    [SerializeField] private float _maxBounceAngle = 60f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _boxCollider.enabled = false;
        
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space) && _isAttached)
        // {
        //     LaunchBall();
        // }
        // if (_isAttached)
        // {
        //     transform.position = new Vector3(_paddle.position.x, _paddle.position.y + _offsetY, 0f);
        // }
    }

    public void BallIsAttached()
    {
        transform.position = new Vector3(_paddle.position.x, _paddle.position.y + _offsetY, 0f);
    }

    public void ResetBallPosition()
    {
        // _isActive = false;
        _isAttached = true;
        _rb.linearVelocity = Vector2.zero;

        if (_paddle != null)
        {
            transform.position = new Vector3(_paddle.position.x, _paddle.position.y + 0.0f, 0.6f);
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }
    public void LaunchBall()
    {
        _isAttached = false;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _boxCollider.enabled = true;

        AddStartingForce();
    }

    private void AddStartingForce()
    {
        //_isActive = true;

        //Unity Editor'da her Play butonuna basışta Random seed'i aynı set edilir (bug repro için). Bu yüzden 2-3 defa play/stop yapınca hep aynı yöne gider!
        //Bu satır her çağrıda farklı seed üretir → artık hep farklı yön!
        UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());

        // Daha doğal bir dağılım için: Sol-sağ arası random x (-1 ile 1 arası), ama sıfır olma (düz yukarı gitmesin)
        float x = UnityEngine.Random.Range(-1f, 1f);
        if (Mathf.Approximately(x, 0f)) //Nadir sıfır durumu yakala
            x = UnityEngine.Random.value < 0.5f ? -0.3f : 0.3f;

        float y = UnityEngine.Random.Range(0.8f, 1f);

        Vector2 _direction = new Vector2(x, y).normalized; // Normalize et: Her seferinde aynı hız
        _rb.AddForce(_direction * _speed);
    }

    public void BoostSpeed(float extraSpeed)
    {
        Vector2 v = _rb.linearVelocity;
        if (v.sqrMagnitude < 0.0001f) return;
        float _newSpeed = v.magnitude + extraSpeed;

        Vector2 dir = v.normalized;

        dir = ClampDirectionAngle(dir, _maxBounceAngle);

        _rb.linearVelocity = dir * _newSpeed;
    }

    private Vector2 ClampDirectionAngle(Vector2 dir, float maxAngleDeg)
    {
        float xSign = Mathf.Sign(dir.x);
        if (xSign == 0) xSign = 1f;

        //dir'i Sağa gidiyormuş gibi düşün (x pozitif)
        float angle = Mathf.Atan2(dir.y, Mathf.Abs(dir.x)); //0.90 derece

        float maxRad = maxAngleDeg * Mathf.Deg2Rad;
        angle = Mathf.Clamp(angle, -maxRad, maxRad);

        // yeni yön: x= cos, y = sin
        float x = Mathf.Cos(angle) * xSign;
        float y = Mathf.Sin(angle);

        return new Vector2(x, y).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.collider.CompareTag("Goal"))
        // {
        //     ResetBallPosition();
        // }

        if (collision.collider.CompareTag("Wall"))
        {
            AudioManager.Instance.PlaySfx(GameAudioLibrary.Instance._wallHit);
        }
        else if (collision.collider.CompareTag("Paddle"))
        {
            AudioManager.Instance.PlaySfx(GameAudioLibrary.Instance._paddleHit);
        }

        // // Paddle Çarpışmasına özel handle et 
        // if (collision.gameObject.CompareTag("Paddle"))
        // {
        //     HandlePaddleBounce(collision);
        //     return; //Physics bouncy'yi ignore ediyoruz (manuel override)
        // }
        // else
        // {
        //     BoostSpeed(0.1f); //Her çarpışmada hafifce hızlanır
        // }
    }


    // private void HandlePaddleBounce(Collision2D collision)
    // {
    //     // çarpışma noktasını alıyoruz
    //     ContactPoint2D contact = collision.contacts[0];

    //     // Paddle'ın bounds'unu alıyoruz (sol/orta/sağ hesapla)
    //     Bounds _paddleBounds = collision.collider.bounds;
    //     float _paddleCenter = _paddleBounds.center.x;
    //     float _hitReletiveX = (contact.point.x - _paddleCenter) / (_paddleBounds.size.x * 0.5f); //-1 (Sol) ile 1 (sağ) arası

    //     //yeni yön: Paddle bias + yukarı
    //     float x = _hitReletiveX * _paddleXVelocityMultiplier; // sol vuruş = negatif x
    //     float y = 1f; //Her zaman yukarı gider

    //     Vector2 newDir = new Vector2(x, y).normalized;

    //     //Clamp uyguluyoruz (60f Derece sınırı)
    //     newDir = ClampDirectionAngle(newDir, _maxBounceAngle);

    //     //Hız uyguluyoruz (mevcut hız + paddle boost)
    //     float _currentSpeed = _rb.linearVelocity.magnitude;
    //     float _newSpeed = _currentSpeed * _paddleHitStrength;
    //     _rb.linearVelocity = newDir * _newSpeed;
    // }
    
}
