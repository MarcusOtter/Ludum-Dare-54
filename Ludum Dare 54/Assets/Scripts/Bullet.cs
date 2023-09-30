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

    public void ModifyHealth(int delta)
    {
        // Slowmo experiments
        // if (delta > 0)
        // {
        //     Time.timeScale = 0.2f;
        //     Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //     this.FireAndForgetWithDelay(0.02f, () =>
        //     {
        //         // This has a problem because we can be holding and then timescale should be 0.5
        //         Time.timeScale = 1f;
        //         Time.fixedDeltaTime = 0.02f;
        //     });
        // }
        
        health += delta;
        if (health > 0) return;
        
        rigidbody2D.velocity = Vector2.zero;
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.enabled = false;
        }

        Destroy(gameObject, 0.2f);
    }
}
