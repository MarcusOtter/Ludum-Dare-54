using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class GameUiManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI totalScoreText;
	[SerializeField] private TextMeshProUGUI spacePressesText;
	[SerializeField] private TMP_Text scoreTextPrefab;

	private GameStateManager _gameStateManager;
	private Dictionary<int, List<TMP_Text>> _scoreTexts = new();

	private void OnEnable()
	{
		_gameStateManager = FindAnyObjectByType<GameStateManager>();
		_gameStateManager.OnPressesRemainingChanged += HandlePressesRemainingChanged;
		_gameStateManager.OnScoreChanged += HandleScoreChanged;
		Bullet.OnBulletDestroyed += HandleBulletDestroyed;
	}

	public void SpawnScoreText(Bullet bullet, int score)
	{
		var text = Instantiate(scoreTextPrefab, bullet.transform.position, Quaternion.identity);
		text.text = $"+{score}";

		
		if (!_scoreTexts.ContainsKey(bullet.GetInstanceID()))
		{
			_scoreTexts.Add(bullet.GetInstanceID(), new List<TMP_Text>());
		}
		
		_scoreTexts[bullet.GetInstanceID()].Add(text);
	}
	
	private void DeleteTextsForBullet(Bullet bullet)
	{
		if (!_scoreTexts.ContainsKey(bullet.GetInstanceID()))
		{
			return;
		}
		
		foreach (var text in _scoreTexts[bullet.GetInstanceID()])
		{
			Destroy(text.gameObject);
		}
		
		_scoreTexts.Remove(bullet.GetInstanceID());
	}

	private void HandleBulletDestroyed(Bullet bullet, [CanBeNull] Target target)
	{
		if (!_scoreTexts.TryGetValue(bullet.GetInstanceID(), out var texts)) return;

		if (target != null)
		{
			foreach (var scoreText in texts)
			{
				// TODO: Some animation
				var score = int.Parse(scoreText.text);
				var newScore = score * (bullet.HasBounced
					? bullet.bounceTargetHitScoreMultiplier
					: bullet.directTargetHitScoreMultiplier);
				scoreText.text = $"+{newScore}";
			}
		}

		this.FireAndForgetWithDelay(0.5f, () =>
		{
			DeleteTextsForBullet(bullet);
		});
	}

	private void HandlePressesRemainingChanged(int remaining)
	{
		spacePressesText.text = Mathf.Max(remaining, 0).ToString();
	}
	
	private void HandleScoreChanged(int totalScore)
	{
		totalScoreText.text = totalScore.ToString();
	}
	
	private void OnDisable()
	{
		_gameStateManager.OnPressesRemainingChanged -= HandlePressesRemainingChanged;
		_gameStateManager.OnScoreChanged -= HandleScoreChanged;
		Bullet.OnBulletDestroyed -= HandleBulletDestroyed;
	}
}
