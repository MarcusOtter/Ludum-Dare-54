using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private float speed = 2f;
    [SerializeField] private int degreesPerTurn = 45;
    
    private Rigidbody2D _rigidbody;
    private int _animationHash;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animationHash = Animator.StringToHash("ButtonPress");
        _rigidbody.velocity = transform.up * speed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            transform.up = Quaternion.Euler(0, 0, degreesPerTurn) * transform.up;
            _rigidbody.velocity = transform.up * speed;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }

        animator.SetBool(_animationHash, Input.GetKey(KeyCode.Space));  
    }
    
    public void AlignWithVelocity()
    {
        transform.up = _rigidbody.velocity.normalized;
    }
}
