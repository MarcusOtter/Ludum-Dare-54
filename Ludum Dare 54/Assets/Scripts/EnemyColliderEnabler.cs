using UnityEngine;

public class EnemyColliderEnabler : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.TryGetComponent<Enemy>(out var enemy))
		{
			return;
		}
		
		enemy.EnableCollider();
	}
}
