using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private float minCameraSize = 4.5f;
    [SerializeField] private float cameraZoomInSpeed = 2f;
    [SerializeField] private float cameraZoomOutSpeed = 20f;
    [SerializeField] private float directionalShakeRecoverySpeed = 10f;
    [SerializeField] private float bulletBounceShakeStrength = 0.015f;
    [SerializeField] private float targetHitShakeStrength = 0.2f;

    private PlayerInput _playerInput;
    private float _startCameraSize;

    private void Awake()
    {
        _playerInput = FindAnyObjectByType<PlayerInput>();
        _startCameraSize = camera.orthographicSize;
    }
    
    private void OnEnable()
    {
        Wall.OnCollisionWithBullet += HandleWallCollisionWithBullet;
        Target.OnTargetHit += HandleTargetHit;
    }

    private void Update()
    {
        camera.orthographicSize = _playerInput.IsSpacebarPressed
            ? Mathf.Lerp(camera.orthographicSize, minCameraSize, cameraZoomInSpeed * Time.deltaTime)
            : Mathf.Lerp(camera.orthographicSize, _startCameraSize, cameraZoomOutSpeed * Time.deltaTime);
        
        transform.position = Vector3.Lerp(transform.position, Vector3.zero, directionalShakeRecoverySpeed * Time.deltaTime);
    }
    
    private void HandleWallCollisionWithBullet(Wall wall, Bullet bullet)
    {
        var direction = -wall.transform.up;
        transform.position += direction * bulletBounceShakeStrength;
    }
    
    private void HandleTargetHit(Target target, Bullet bullet)
    {
        var direction = -target.transform.up;
        var strength = bullet.HasBounced ? bulletBounceShakeStrength * 3 : targetHitShakeStrength;
        transform.position += direction * strength;
    }
    
    private void OnDisable()
    {
        Wall.OnCollisionWithBullet -= HandleWallCollisionWithBullet;
        Target.OnTargetHit -= HandleTargetHit;
    }
}
