using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
	[SerializeField] private Animator animator;
	
	private PlayerInput _playerInput;

	private readonly int _spaceDownHash = Animator.StringToHash("SpaceKeyDown");
	private readonly int _spaceUpHash = Animator.StringToHash("SpaceKeyUp");
	
	private void OnEnable()
	{
		_playerInput = FindAnyObjectByType<PlayerInput>();
		_playerInput.OnSpacebarDown += HandleSpacebarDown;
		_playerInput.OnSpacebarUp += HandleSpacebarUp;
	}

	private void HandleSpacebarDown()
	{
		animator.SetTrigger(_spaceDownHash);
	}
	
	private void HandleSpacebarUp()
	{
		animator.SetTrigger(_spaceUpHash);
	}

	private void OnDisable()
	{
		_playerInput.OnSpacebarDown -= HandleSpacebarDown;
		_playerInput.OnSpacebarUp -= HandleSpacebarUp;
	}
}
