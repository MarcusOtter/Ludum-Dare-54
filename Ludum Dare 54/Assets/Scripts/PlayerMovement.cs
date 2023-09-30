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
    }

    private void Update()
    {
        _rigidbody.velocity = transform.up * speed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.MoveRotation(_rigidbody.rotation + degreesPerTurn);
        }
        
        animator.SetBool(_animationHash, Input.GetKey(KeyCode.Space));  
    }
}
