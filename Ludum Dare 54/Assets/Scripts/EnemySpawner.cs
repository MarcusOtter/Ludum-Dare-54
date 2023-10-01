using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    
    [Header("Settings")]
    [SerializeField] private Vector2 slideRange = new(-2.5f, 2.5f);
    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private int enemiesPerGroup = 5;
    [SerializeField] private float groupGapSize = 1f;
    [SerializeField] private float spawnDistance = 10f;

    private GameStateManager _gameStateManager;
    
    private float _timeSinceLastGroupSpawn;

    private void Awake()
    {
        _gameStateManager = FindAnyObjectByType<GameStateManager>();
        SpawnGroup();
    }
    
    private void Update()
    {
        if (_gameStateManager.SpacePressesRemaining <= 0) return;
        _timeSinceLastGroupSpawn += Time.deltaTime;

        if (_timeSinceLastGroupSpawn >= spawnDelay)
        {
            _timeSinceLastGroupSpawn = 0f;
            SpawnGroup();
        }
    }

    private void SpawnGroup()
    {
        spawnPoint.position = transform.position;
        spawnPoint.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        
        spawnPoint.position += spawnPoint.up * -spawnDistance;
        spawnPoint.position += spawnPoint.right * Random.Range(slideRange.x, slideRange.y);
        
        
        for(var i = 0; i < enemiesPerGroup; i++)
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            spawnPoint.position += spawnPoint.up * -groupGapSize;
        }
    }
}
