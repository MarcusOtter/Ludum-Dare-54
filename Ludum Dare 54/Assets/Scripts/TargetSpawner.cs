using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetSpawner : MonoBehaviour
{
	[SerializeField] private Target targetPrefab;
	[SerializeField] private Transform[] spawnPoints;
	[SerializeField] private Vector2 slideMinMax;
	[SerializeField] private float spawnDelay;
	[SerializeField] private int maxTargets;

	private float _timeSinceLastSpawn;
	private int _activeTargets;
	private List<Transform> _activeSpawnPoints;

	// The int is the instance ID of the target that was spawned here
	private readonly Dictionary<int, Transform> _inactiveSpawnPoints = new ();

	private void Awake()
	{
		_activeSpawnPoints = new List<Transform>(spawnPoints);
	}

	private void OnEnable()
	{
		Target.OnTargetHit += HandleTargetHit;
		
		SpawnTarget();
		SpawnTarget();
	}

	private void Update()
	{
		_timeSinceLastSpawn += Time.deltaTime;

		if (_timeSinceLastSpawn >= spawnDelay)
		{
			_timeSinceLastSpawn = 0f;
			SpawnTarget();
		}
	}

	private void HandleTargetHit(Target hitTarget, Bullet _)
	{
		var spawnPoint = _inactiveSpawnPoints[hitTarget.GetHashCode()];
		_activeSpawnPoints.Add(spawnPoint);
		_inactiveSpawnPoints.Remove(hitTarget.GetHashCode());
		_activeTargets--;
		Destroy(hitTarget.gameObject);
	}
	
	private void SpawnTarget()
	{
		if (_activeTargets >= maxTargets) return;
		
		var spawnPoint = _activeSpawnPoints[Random.Range(0, _activeSpawnPoints.Count)];
		var target = Instantiate(targetPrefab, spawnPoint.position, spawnPoint.rotation);
		var slideAmount = Random.Range(slideMinMax.x, slideMinMax.y);
		target.transform.position += target.transform.right * slideAmount;
		
		_activeSpawnPoints.Remove(spawnPoint);
		_inactiveSpawnPoints.Add(target.GetHashCode(), spawnPoint);
		
		_activeTargets++;
	}

	private void OnDisable()
	{
		Target.OnTargetHit -= HandleTargetHit;
	}
}
