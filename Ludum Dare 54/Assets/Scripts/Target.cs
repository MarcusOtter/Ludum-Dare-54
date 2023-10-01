using System;
using UnityEngine;

public class Target : MonoBehaviour
{
	public static event Action<Target, Bullet> OnTargetHit;
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.TryGetComponent<Bullet>(out var bullet))
		{
			return;
		}
		
		// TODO: Play some cool effect?
		bullet.ModifyHealth(-9999999);
		OnTargetHit?.Invoke(this, bullet);
	}
}
