using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static event Action<Bullet, Target> OnBulletDestroyed;

    public bool HasBounced { get; private set; }
    public Color Color => HasBounced ? afterBounceColor : beforeBounceColor;

    public int bounceTargetHitScoreMultiplier = 2;
    public int directTargetHitScoreMultiplier = 10;
    
    [SerializeField] private new Collider2D collider;
    [SerializeField] private new Rigidbody2D rigidbody2D;
    [SerializeField] private float speed = 30f;
    [SerializeField] private float health = 2;
    [SerializeField] private int enemyKillBaseScore = 10;
    [SerializeField] private Color beforeBounceColor;
    [SerializeField] private Color afterBounceColor;
    [SerializeField] private SoundEffect killSoundEffectBase;
    [SerializeField] private SoundEffect[] killSoundEffectMulti;
    [SerializeField] private float pitchPerKill = 0.1f;

    private GameUiManager _gameUiManager;
    private SpriteRenderer[] _spriteRenderers;
    private TrailRenderer _trailRenderer;

    private int _kills;
    private bool _hitTarget;

    private void Awake()
    {
        rigidbody2D.velocity = transform.up * speed;
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _gameUiManager = FindAnyObjectByType<GameUiManager>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
        SetColor(beforeBounceColor);
    }

    public void SetHasBounced()
    {
        if (_hitTarget) return; // Color is locked in after hitting target
        HasBounced = true;
        SetColor(afterBounceColor);
    }

    public void LogKill(Enemy _)
    {
        _kills++;
        // killSoundEffectBase.Play();
        // var multi = killSoundEffectMulti[Mathf.Min(_kills - 1, killSoundEffectMulti.Length - 1)];
        var newAudioSource = new GameObject("KillSoundEffect").AddComponent<AudioSource>();
        newAudioSource.pitch = 1 + _kills * pitchPerKill;
        
        newAudioSource.PlayOneShot(killSoundEffectBase.Clips[0], killSoundEffectBase.MinVolume);
        
        AddScore();
    }

    public void LogTargetHit(Target target)
    {
        _hitTarget = true;
        HideBullet();
        Destroy(gameObject, 3f);
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
        
        HideBullet();
        Destroy(gameObject, 3f);
        OnBulletDestroyed?.Invoke(this, null);
    }

    private void HideBullet()
    {
        rigidbody2D.velocity = Vector2.zero;
        collider.enabled = false;
        
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.enabled = false;
        }
        
        if (_trailRenderer)
        {
            _trailRenderer.material.color = Color.clear;
        }
    }
    
    private void SetColor(Color color)
    {
        foreach(var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.color = color;
        }

        if (_trailRenderer)
        {
            _trailRenderer.material.color = color;
        }
    }
    
    private void AddScore()
    {
        var scoreDelta = enemyKillBaseScore * _kills;
        _gameUiManager.SpawnScoreText(this, scoreDelta);
    }
}
