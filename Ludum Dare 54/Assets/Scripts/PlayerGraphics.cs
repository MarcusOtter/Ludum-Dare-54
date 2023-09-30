using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private string _spaceBoolName;
	
	private PlayerInput _playerInput;

	private int _spaceBoolHash;
	
	private void OnEnable()
	{
		_playerInput = FindAnyObjectByType<PlayerInput>();
		_playerInput.OnSpacebarDown += HandleSpacebarDown;
		_playerInput.OnSpacebarUp += HandleSpacebarUp;
		
		_spaceBoolHash = Animator.StringToHash(_spaceBoolName);
	}

	private void HandleSpacebarDown()
	{
		_animator.SetBool(_spaceBoolHash, true);
	}
	
	private void HandleSpacebarUp()
	{
		_animator.SetBool(_spaceBoolHash, false);
	}

	private void OnDisable()
	{
		_playerInput.OnSpacebarDown -= HandleSpacebarDown;
		_playerInput.OnSpacebarUp -= HandleSpacebarUp;
	}
}
