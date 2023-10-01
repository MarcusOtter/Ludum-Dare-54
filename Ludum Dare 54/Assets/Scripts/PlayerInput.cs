using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public event Action OnSpacebarDown;
	public event Action OnSpacebarUp;
	public bool IsSpacebarPressed { get; private set; }

	private GameStateManager _gameStateManager;
	
	private void Awake()
	{
		_gameStateManager = FindAnyObjectByType<GameStateManager>();
	}
	
	// ReSharper disable Unity.PerformanceAnalysis
	private void Update()
	{
		if (_gameStateManager.SpacePressesRemaining <= 0)
		{
			if (IsSpacebarPressed)
			{
				OnSpacebarUp?.Invoke();
			}
			
			IsSpacebarPressed = false;
			return;
		}
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			IsSpacebarPressed = true;
			OnSpacebarDown?.Invoke();
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			IsSpacebarPressed = false;
			OnSpacebarUp?.Invoke();
		}
	}
}
