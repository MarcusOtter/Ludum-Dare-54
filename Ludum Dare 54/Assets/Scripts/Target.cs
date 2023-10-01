using System;
using UnityEngine;

public class Target : MonoBehaviour
{
	public static event Action<Target, Bullet> OnTargetHit;

	[SerializeField] private Animator animator;
	[SerializeField] private string directHitTriggerName = "DirectHit";
	[SerializeField] private string bounceHitTriggerName = "BounceHit";

	private int _directHitTriggerHash;
	private int _bounceHitTriggerHash;

	private void Awake()
	{
		_directHitTriggerHash = Animator.StringToHash(directHitTriggerName);
		_bounceHitTriggerHash = Animator.StringToHash(bounceHitTriggerName);
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.TryGetComponent<Bullet>(out var bullet))
		{
			return;
		}
		
		animator.SetTrigger(bullet.HasBounced ? _bounceHitTriggerHash : _directHitTriggerHash);
		
		bullet.LogTargetHit(this);
		OnTargetHit?.Invoke(this, bullet);
	}
}
