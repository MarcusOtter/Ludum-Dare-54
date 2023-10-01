using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static event Action<Bullet, Target> OnBulletDestroyed;

    public int Score { get; private set; }

    public bool HasBounced { get; private set; }

    public int bounceTargetHitScoreMultiplier = 2;
    public int directTargetHitScoreMultiplier = 10;
    
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private float speed = 30f;
    [SerializeField] private float health = 2;
    [SerializeField] private int enemyKillBaseScore = 10;

    private GameUiManager _gameUiManager;
    private SpriteRenderer[] _spriteRenderers;
    
    private int _kills;

    private void Awake()
    {
        rigidbody2D.velocity = transform.up * speed;
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _gameUiManager = FindAnyObjectByType<GameUiManager>();
    }

    public void SetHasBounced()
    {
        HasBounced = true;
    }

    public void LogKill(Enemy _)
    {
        _kills++;
        AddScore();
    }

    public void LogTargetHit(Target target)
    {
        Score *= HasBounced ? bounceTargetHitScoreMultiplier : directTargetHitScoreMultiplier;
        OnBulletDestroyed?.Invoke(this, target);
    }
    
    public void ModifyHealth(float delta)
    {
        // Slowmo experiments
        // if (delta > 0)
        // {
        //     Time.timeScale = 0.2f;
        //     Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //     this.FireAndForgetWithDelay(0.02f, () =>
        //     {
        //         // This has a problem because we can be holding and then timescale should be 0.5
        //         Time.timeScale = 1f;
        //         Time.fixedDeltaTime = 0.02f;
        //     });
        // }
        
        health += delta;
        if (health > 0) return;
        
        rigidbody2D.velocity = Vector2.zero;
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.enabled = false;
        }

        OnBulletDestroyed?.Invoke(this, null);
        Destroy(gameObject, 0.2f);
    }
    
    private void AddScore()
    {
        var scoreDelta = enemyKillBaseScore * _kills;
        Score += scoreDelta;
        _gameUiManager.SpawnScoreText(this, scoreDelta);
    }
}
