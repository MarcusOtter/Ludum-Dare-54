using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public static event Action<Enemy> OnCollisionWithPlayer;
	public static event Action<Enemy, Bullet> OnCollisionWithBullet;
	
	[SerializeField] private new Rigidbody2D rigidbody;
	[SerializeField] private new Collider2D collider;
	[SerializeField] private float speed;
	[SerializeField] private float bulletHealthReward = 0.5f;
	[SerializeField] private SoundEffect playerHurtSound;
	
	private void Awake()
	{
		rigidbody.velocity = transform.up * speed;
	}
	
	public void EnableCollider()
	{
		collider.enabled = true;
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.TryGetComponent<Bullet>(out var bullet))
		{
			bullet.ModifyHealth(bulletHealthReward);
			bullet.LogKill(this);
			OnCollisionWithBullet?.Invoke(this, bullet);
			Die();
		}
		
		if (other.TryGetComponent<PlayerMovement>(out _))
		{
			playerHurtSound.Play();
			OnCollisionWithPlayer?.Invoke(this);
			Die();
		}
	}

	private void Die()
	{
		// TODO: Death animation & sounds, depending on direction of bullet
		Destroy(gameObject);
	}
}
