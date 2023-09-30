using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;

    private PlayerInput _playerInput;

    private void OnEnable()
    {
        _playerInput = FindAnyObjectByType<PlayerInput>();
        _playerInput.OnSpacebarUp += HandleSpacebarUp;
    }

    private void HandleSpacebarUp()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
    
    private void OnDisable()
    {
        _playerInput.OnSpacebarUp -= HandleSpacebarUp;
    }
}
