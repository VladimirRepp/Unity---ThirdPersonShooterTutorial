using System;
using UnityEngine;

/// <summary>
/// Физический объект - пуля 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private float _maxLifetime;
    private float _damage;
    private Rigidbody _rb;
    private BulletPool _pool;

    private float _timeElapsed = 0;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CountdownLifetime();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Пуля маленькая - берем первое попадание 
        ContactPoint contact = collision.contacts[0];

        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;

        IDamageable damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage, hitPoint, hitNormal);
        }

        _pool.ReturnBullet(this);
    }

    public void Init(float damage, Vector3 direction, float speed, BulletPool pool, float maxLifetime)
    {
        _damage = damage;
        _pool = pool;
        _maxLifetime = maxLifetime;

        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(direction * speed, ForceMode.Impulse);
    }

    private void CountdownLifetime()
    {
        _timeElapsed += Time.deltaTime;

        if (_timeElapsed >= _maxLifetime)
            _pool.ReturnBullet(this);
    }
}
