using UnityEngine;

/// <summary>
/// Weapon — базовый компонент оружия. 
/// Weapon не стреляет сам.
/// Он лишь делегирует стрельбу компоненту IShooter
/// </summary>
public class Weapon : MonoBehaviour
{
    [Header("Common Settings")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private Transform shootPoint;

    // Raycast:
    // камера → попадание
    // Projectile:
    // камера → точка
    // ствол → точка

    // Камера определяет точку прицеливания
    // Пуля летит из ствола в эту точку

    // Физическая пуля должна лететь не “вперёд из ствола”, а в точку, куда смотрит камера.

    private float _nextFireTime;
    private IShooter _shooter;

    public Transform ShootPoint => shootPoint;
    public float Damage => damage;

    private void Awake()
    {
        _shooter = GetComponent<IShooter>();
    }

    public void TryShoot()
    {
        if (Time.time < _nextFireTime)
            return;

        _nextFireTime = Time.time + fireRate;
        _shooter.Shoot();
    }
}
