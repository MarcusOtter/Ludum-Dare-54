using TMPro;
using UnityEngine;

public class GameUiManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI spacePressesText;
	
	private GameStateManager _gameStateManager;

	private void OnEnable()
	{
		_gameStateManager = FindAnyObjectByType<GameStateManager>();
		_gameStateManager.OnPressesRemainingChanged += HandlePressesRemainingChanged;
	}

	private void HandlePressesRemainingChanged(int remaining)
	{
		spacePressesText.text = Mathf.Max(remaining, 0).ToString();
	}
	
	private void OnDisable()
	{
		_gameStateManager.OnPressesRemainingChanged -= HandlePressesRemainingChanged;
	}
}
