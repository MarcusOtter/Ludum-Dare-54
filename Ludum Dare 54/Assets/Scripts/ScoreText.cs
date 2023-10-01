using System;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
	public static event Action<int> OnScoreAnimationFinished; 

	[SerializeField] private TMP_Text text;
	[SerializeField] private Animator animator;

	private static readonly int NormalScoreTriggerHash = Animator.StringToHash("Normal");
	private static readonly int BounceScoreTriggerHash = Animator.StringToHash("Bounce");
	private static readonly int DirectHitScoreTriggerHash = Animator.StringToHash("DirectHit");
	
	private int _score;
	
	public void SetScore(int score)
	{
		_score = score;
		text.text = score.ToString();
	}

	public void TriggerBulletDeath(Bullet bullet, Target target, float delay)
	{
		this.FireAndForgetWithDelay(delay, () =>
		{
			if (target == null)
			{
				animator.SetTrigger(NormalScoreTriggerHash);
			}
			else
			{
				animator.SetTrigger(bullet.HasBounced ? BounceScoreTriggerHash : DirectHitScoreTriggerHash);
			}
			
			_score *= bullet.HasBounced
				? bullet.bounceTargetHitScoreMultiplier
				: bullet.directTargetHitScoreMultiplier;
			
			text.text = _score.ToString();

			OnScoreAnimationFinished?.Invoke(_score);
		});
	}
}
