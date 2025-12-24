using UnityEngine;

/// <summary>
/// Стрелок физическими пулями 
/// </summary>
public class ProjectileShooter : MonoBehaviour, IShooter
{
    [Header("Reference Settingss")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private BulletPool bulletPool;

    [Header("Projectile Settings")]
    [SerializeField] private float bulletSpeed = 30f;
    [SerializeField] private float maxAimDistance = 100f;
    [SerializeField] private LayerMask aimMask;

    private Weapon _weapon;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    public void Shoot()
    {
        Vector3 aimPoint = GetAimPointFromCamera();
        Vector3 direction = (aimPoint - shootPoint.position).normalized;

        Bullet bullet = bulletPool.GetBullet();
        bullet.transform.position = shootPoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(direction);

        bullet.Init(
            _weapon.Damage,
            direction,
            bulletSpeed,
            bulletPool,
            bulletPool.MaxLifetime
        );
    }

    private Vector3 GetAimPointFromCamera()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimMask))
            return hit.point;

        return ray.origin + ray.direction * maxAimDistance;
    }
}
