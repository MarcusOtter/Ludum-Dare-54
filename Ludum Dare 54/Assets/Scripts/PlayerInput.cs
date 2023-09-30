using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public event Action OnSpacebarDown;
	public event Action OnSpacebarUp;
	public bool IsSpacebarPressed { get; private set; }
	
	// ReSharper disable Unity.PerformanceAnalysis
	private void Update()
	{
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
