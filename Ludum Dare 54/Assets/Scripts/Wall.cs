using System;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public static event Action<Wall, Bullet> OnCollisionWithBullet;

    [SerializeField] private SpriteRenderer outlineRenderer;
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashDuration = 0.1f;
    
    private Color _outlineStartColor;

    private void Awake()
    {
        _outlineStartColor = outlineRenderer.color;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerMovement>(out var player))
        {
            if (!player.IsSpinning)
            {
                player.AlignWithVelocity();
                return;
            }
        }
        
        if (other.gameObject.TryGetComponent<Bullet>(out var bullet))
        {
            // Flashing was kinda eh
            //FlashColor(bullet.Color);
            bullet.ModifyHealth(-1);
            bullet.SetHasBounced();
            OnCollisionWithBullet?.Invoke(this, bullet);
            return;
        }
    }

    private void FlashColor(Color color)
    {
        // Not using bullet color atm
        outlineRenderer.color = flashColor;

        this.FireAndForgetWithDelay(flashDuration, () =>
        {
            outlineRenderer.color = _outlineStartColor;
        });
    }
}
