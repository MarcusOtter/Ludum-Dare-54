using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
	public event Action<int> OnPressesRemainingChanged;
	public event Action<int> OnScoreChanged;
	public int SpacePressesRemaining => _spacePressesRemaining;
	
	[Header("Space presses (health)")]
	[SerializeField] private int startingSpacePresses = 25;
	[SerializeField] private float holdTimePerPress = 0.5f;
	[SerializeField] private int targetDirectHitReward = 3;
	[SerializeField] private int targetBounceHitReward = 1;
	[SerializeField] private int damageOnEnemyCollision = 10;

	private PlayerInput _playerInput;
	
	private int _spacePressesRemaining;
	private int _score;
	private float _holdTime;

	private void Awake()
	{
		_spacePressesRemaining = startingSpacePresses;
		OnPressesRemainingChanged?.Invoke(startingSpacePresses);
		OnScoreChanged?.Invoke(0);
	}

	private void OnEnable()
	{
		_playerInput = FindAnyObjectByType<PlayerInput>();
		_playerInput.OnSpacebarDown += HandleSpacebarDown;
		Target.OnTargetHit += HandleTargetHit;
		Enemy.OnCollisionWithPlayer += HandleEnemyCollisionWithPlayer;
		Bullet.OnBulletDestroyed += HandleBulletDestroyed;
	}

	private void Update()
	{
		if (!_playerInput.IsSpacebarPressed) return;
		
		_holdTime += Time.deltaTime;
		if (_holdTime < holdTimePerPress) return;
		
		_holdTime = 0;
		UpdatePressesRemaining(-1);
	}
	
	private void HandleSpacebarDown()
	{
		_holdTime = 0;
		UpdatePressesRemaining(-1);
	}
	
	private void HandleTargetHit(Target target, Bullet bullet)
	{
		UpdatePressesRemaining(bullet.HasBounced ? targetBounceHitReward : targetDirectHitReward);
	}
	
	private void HandleEnemyCollisionWithPlayer(Enemy _)
	{
		UpdatePressesRemaining(-damageOnEnemyCollision);
	}
	
	private void HandleBulletDestroyed(Bullet bullet, Target _)
	{
		UpdateScore(bullet.Score);
	}

	private void UpdatePressesRemaining(int delta)
	{
		_spacePressesRemaining += delta;
		OnPressesRemainingChanged?.Invoke(_spacePressesRemaining);
	}
	
	private void UpdateScore(int delta)
	{
		_score += delta;
		OnScoreChanged?.Invoke(_score);
	}

	private void OnDisable()
	{
		_playerInput.OnSpacebarDown -= HandleSpacebarDown;
		Target.OnTargetHit -= HandleTargetHit;
		Enemy.OnCollisionWithPlayer -= HandleEnemyCollisionWithPlayer;
		Bullet.OnBulletDestroyed -= HandleBulletDestroyed;
	}
}
