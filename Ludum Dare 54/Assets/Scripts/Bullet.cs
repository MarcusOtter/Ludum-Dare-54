using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private float speed;

    [SerializeField] private int health = 2;

    private SpriteRenderer[] _spriteRenderers;
    
    private void Awake()
    {
        rigidbody2D.velocity = transform.up * speed;
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health > 0) return;
        
        rigidbody2D.velocity = Vector2.zero;
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.enabled = false;
        }
        Destroy(gameObject, 0.2f);
    }
}
