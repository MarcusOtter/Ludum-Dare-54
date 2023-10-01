using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
	public event Action<int> OnPressesRemainingChanged;
	public event Action<int> OnScoreChanged;
	
	public int SpacePressesRemaining { get; private set; }

	public UnityEvent OnGameOver;
	
	[Header("Space presses (health)")]
	[SerializeField] private int startingSpacePresses = 25;
	[SerializeField] private float holdTimePerPress = 0.5f;
	[SerializeField] private int targetDirectHitReward = 3;
	[SerializeField] private int targetBounceHitReward = 1;
	[SerializeField] private int damageOnEnemyCollision = 10;
	[SerializeField] private float restartDelay = 2f;
	[SerializeField] private GameObject spaceToRestart;
	[SerializeField] private TextMeshProUGUI highscoreText;

	private PlayerInput _playerInput;

	private int _score;
	private int _highscore;
	private float _holdTime;

	private void Awake()
	{
		_highscore = PlayerPrefs.GetInt("highscore", 0);
		SpacePressesRemaining = startingSpacePresses;
		OnPressesRemainingChanged?.Invoke(startingSpacePresses);
		OnScoreChanged?.Invoke(0);
	}

	private void OnEnable()
	{
		_playerInput = FindAnyObjectByType<PlayerInput>();
		_playerInput.OnSpacebarDown += HandleSpacebarDown;
		Target.OnTargetHit += HandleTargetHit;
		Enemy.OnCollisionWithPlayer += HandleEnemyCollisionWithPlayer;
		ScoreText.OnScoreAnimationFinished += HandleScoreAnimationFinished;
	}

	private void Update()
	{
		if (SpacePressesRemaining <= 0)
		{
			restartDelay -= Time.deltaTime;
			if (restartDelay <= 0)
			{
				spaceToRestart.SetActive(true);
				if (Input.GetKeyDown(KeyCode.Space))
				{
					SimpleSceneManager.ReloadScene();
				}
			}
		}
		
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

	private void HandleScoreAnimationFinished(int scoreDelta)
	{
		UpdateScore(scoreDelta);
	}

	private void UpdatePressesRemaining(int delta)
	{
		SpacePressesRemaining += delta;
		OnPressesRemainingChanged?.Invoke(SpacePressesRemaining);
		
		if (SpacePressesRemaining <= 0)
		{
			_highscore = Mathf.Max(_highscore, _score);
			highscoreText.text = _highscore.ToString();
			PlayerPrefs.SetInt("highscore", _highscore);
			PlayerPrefs.Save();
			
			OnGameOver?.Invoke();
		}
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
		ScoreText.OnScoreAnimationFinished -= HandleScoreAnimationFinished;
	}
}
