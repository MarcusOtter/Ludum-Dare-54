using UnityEngine;

public class Wall : MonoBehaviour
{
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
            bullet.ModifyHealth(-1);
            return;
        }
    }
}
