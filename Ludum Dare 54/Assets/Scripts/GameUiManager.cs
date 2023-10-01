using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class GameUiManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI totalScoreText;
	[SerializeField] private TextMeshProUGUI spacePressesText;
	[SerializeField] private ScoreText scoreTextPrefab;
	[SerializeField] private float scoreTextStaggerDelay = 0.1f;

	private GameStateManager _gameStateManager;
	private readonly Dictionary<int, List<ScoreText>> _scoreTexts = new();

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
		text.SetScore(score);

		if (!_scoreTexts.ContainsKey(bullet.GetInstanceID()))
		{
			_scoreTexts.Add(bullet.GetInstanceID(), new List<ScoreText>());
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
		
		for (var i = 0; i < texts.Count; i++)
		{
			texts[i].TriggerBulletDeath(bullet, target, scoreTextStaggerDelay * i);
		}

		this.FireAndForgetWithDelay(texts.Count * scoreTextStaggerDelay + 2, () =>
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
