using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public bool IsSpinning => _playerInput.IsSpacebarPressed;
    
    [SerializeField] private float speed = 2f;
    [SerializeField] private float turnSpeed = 1f;

    private PlayerInput _playerInput;
    private Rigidbody2D _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerInput = FindAnyObjectByType<PlayerInput>();
        _rigidbody.velocity = transform.up * speed;
    }

    private void OnEnable()
    {
        _playerInput.OnSpacebarDown += HandleSpacebarDown;
        _playerInput.OnSpacebarUp += HandleSpacebarUp;
    }

    private void Update()
    {
        if (!_playerInput.IsSpacebarPressed) return;
        
        transform.up = Quaternion.Euler(0, 0, -turnSpeed) * transform.up;
        _rigidbody.velocity = Vector2.zero;
    }
    
    public void AlignWithVelocity()
    {
        transform.up = _rigidbody.velocity.normalized;
    }

    private void HandleSpacebarDown()
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    private void HandleSpacebarUp()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        _rigidbody.velocity = transform.up * speed;
    }
    
    private void OnDisable()
    {
        _playerInput.OnSpacebarDown -= HandleSpacebarDown;
        _playerInput.OnSpacebarUp -= HandleSpacebarUp;
    }
}
