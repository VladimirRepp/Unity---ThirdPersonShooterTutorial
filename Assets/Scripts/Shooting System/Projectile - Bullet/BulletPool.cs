using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Пул объектов - пули 
/// </summary>
public class BulletPool : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private float maxLifetime = 5; // в секундах 

    private Queue<Bullet> _pool = new Queue<Bullet>();

    public float MaxLifetime => maxLifetime;

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateBullet();
        }
    }

    private void CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform);
        bullet.gameObject.SetActive(false);
        _pool.Enqueue(bullet);
    }

    public Bullet GetBullet()
    {
        if (_pool.Count == 0)
            CreateBullet();

        Bullet bullet = _pool.Dequeue();
        bullet.gameObject.SetActive(true);
        return bullet;
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        _pool.Enqueue(bullet);
    }
}
