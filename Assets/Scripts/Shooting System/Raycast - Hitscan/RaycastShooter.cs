using UnityEngine;

/// <summary>
/// Стрельба через луч 
/// </summary>
public class RaycastShooter : MonoBehaviour, IShooter
{
    public float range = 100f;
    public LayerMask hitMask;

    private Weapon _weapon;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
    }

    public void Shoot()
    {
        // TODO: по направлению в перед или сразу по центу ? 
        Ray ray = new Ray(_weapon.ShootPoint.position, _weapon.ShootPoint.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask))
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_weapon.Damage, hit.point, hit.normal);
            }

            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.2f);
        }
    }
}
