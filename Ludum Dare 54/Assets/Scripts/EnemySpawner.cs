using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Vector2 xRange = new(-2.5f, 2.5f);
    [SerializeField] private Vector2 yRange = new(-2.5f, 2.5f);
    [SerializeField] private float spawnInterval = 0.25f;
    [SerializeField] private int enemiesPerGroup = 5;
    [SerializeField] private float groupInterval = 5f;
    [SerializeField] private float spawnOffset = 10f;
    
    private float _timeSinceLastGroupSpawn;
    private float _timeSinceLastEnemySpawn;
    private int _enemiesSpawnedThisGroup;
    
    private Vector2 _spawnPosition;
    private Quaternion _spawnRotation;

    private void Awake()
    {
        (_spawnPosition, _spawnRotation) = GetGroupSpawnPosition();
    }
    
    private void Update()
    {
        _timeSinceLastGroupSpawn += Time.deltaTime;
        _timeSinceLastEnemySpawn += Time.deltaTime;

        if (_timeSinceLastGroupSpawn >= groupInterval)
        {
            _timeSinceLastGroupSpawn = 0f;
            _enemiesSpawnedThisGroup = 0;
            (_spawnPosition, _spawnRotation) = GetGroupSpawnPosition();
        }
        
        if (_timeSinceLastEnemySpawn >= spawnInterval && _enemiesSpawnedThisGroup < enemiesPerGroup)
        {
            _timeSinceLastEnemySpawn = 0f;
            _enemiesSpawnedThisGroup++;
            SpawnEnemy(_spawnPosition, _spawnRotation);
        }
    }

    private (Vector2, Quaternion) GetGroupSpawnPosition()
    {
        var side = Random.Range(0, 4);
        var spawnPositionNumber = Random.Range(xRange.x, xRange.y);
        
        var positions = new []{
            new Vector2(spawnPositionNumber, spawnOffset),  // top
            new Vector2(-spawnOffset, spawnPositionNumber), // right
            new Vector2(spawnPositionNumber, -spawnOffset), // bottom
            new Vector2(spawnOffset, spawnPositionNumber)   // left
        };
        
        var rotations = new[]{
            Quaternion.Euler(0, 0, 180), // top
            Quaternion.Euler(0, 0, -90), // right
            Quaternion.identity,              // bottom
            Quaternion.Euler(0, 0, 90)   // left
        };
        
        return (positions[side], rotations[side]);
    }
    
    private void SpawnEnemy(Vector2 position, Quaternion rotation)
    {
        var enemy = Instantiate(enemyPrefab, position, rotation);
        enemy.GetComponent<Rigidbody2D>().velocity = enemy.transform.up * enemy.Speed;
    }
}
