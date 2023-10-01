using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
	public event Action<int> OnPressesRemainingChanged;
	public int SpacePressesRemaining => _spacePressesRemaining;
	
	[SerializeField] private int startingSpacePresses = 25;
	[SerializeField] private float holdTimePerPress = 0.5f;
	[SerializeField] private int targetDirectHitReward = 3;
	[SerializeField] private int targetBounceHitReward = 1;
	[SerializeField] private int damageOnEnemyCollision = 10;
	
	private PlayerInput _playerInput;
	
	private int _spacePressesRemaining;
	private float _holdTime;

	private void Awake()
	{
		_spacePressesRemaining = startingSpacePresses;
		OnPressesRemainingChanged?.Invoke(startingSpacePresses);
	}

	private void OnEnable()
	{
		_playerInput = FindAnyObjectByType<PlayerInput>();
		_playerInput.OnSpacebarDown += HandleSpacebarDown;
		Target.OnTargetHit += HandleTargetHit;
		Enemy.OnCollisionWithPlayer += HandleEnemyCollisionWithPlayer;
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

	private void UpdatePressesRemaining(int delta)
	{
		_spacePressesRemaining += delta;
		OnPressesRemainingChanged?.Invoke(_spacePressesRemaining);
	}

	private void OnDisable()
	{
		_playerInput.OnSpacebarDown -= HandleSpacebarDown;
		Target.OnTargetHit -= HandleTargetHit;
		Enemy.OnCollisionWithPlayer -= HandleEnemyCollisionWithPlayer;
	}
}
