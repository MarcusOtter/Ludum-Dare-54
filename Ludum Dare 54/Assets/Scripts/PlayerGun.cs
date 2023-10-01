using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private SoundEffect shootSound;

    private PlayerInput _playerInput;

    private void OnEnable()
    {
        _playerInput = FindAnyObjectByType<PlayerInput>();
        _playerInput.OnSpacebarUp += HandleSpacebarUp;
    }

    private void HandleSpacebarUp()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        shootSound.Play();
    }
    
    private void OnDisable()
    {
        _playerInput.OnSpacebarUp -= HandleSpacebarUp;
    }
}
