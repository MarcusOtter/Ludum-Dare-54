using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private new Rigidbody2D rigidbody;
	[SerializeField] private new Collider2D collider;
	[SerializeField] private float speed;
	
	private void Awake()
	{
		rigidbody.velocity = transform.up * speed;
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.TryGetComponent<Bullet>(out var bullet))
		{
			bullet.ModifyHealth(1);
			
			// TODO: Death animation & sounds, depending on direction of bullet
			Destroy(gameObject);
		}
		
		// TODO: Game over if player touches
	}

	public void EnableCollider()
	{
		collider.enabled = true;
	}
}
