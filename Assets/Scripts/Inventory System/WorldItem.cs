using UnityEngine;

/// <summary>
/// Предмет, находящийся в игровом мире и доступный для взаимодействия
/// </summary>
public class WorldItem : HintInteractionItem
{
    [Header("Item Settings")]
    public ItemSO itemData;

    [Header("Effects Settings")]
    [SerializeField] private ParticleSystem pickupParticles;
    [SerializeField] private AudioClip pickupSound;
    public ItemSO ItemData => itemData;

    private void Start()
    {
        base.Startup();

        // Если префаб не назначен, используем текущий объект
        if (itemData != null && itemData.worldPrefab == null)
        {
            itemData.worldPrefab = gameObject;
        }
    }

    private void PlayPickupEffects()
    {
        bool effectsPlayed = false;

        // Визуальные эффекты
        if (pickupParticles != null)
        {
            ParticleSystem particles = Instantiate(pickupParticles,
                transform.position, Quaternion.identity);
            effectsPlayed = true;
        }
        else
        {
            effectsPlayed = true;
        }

        // Звуковые эффекты
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        if (effectsPlayed)
        {
            Destroy(gameObject);
        }
    }

    public void Pickup()
    {
        PlayPickupEffects();
        Destroy(gameObject);
    }

    // Метод для настройки предмета (используется при спавне)
    public void Setup(ItemSO data)
    {
        itemData = data;

        // Обновляем внешний вид
        if (data.worldPrefab != null)
        {
            // Можно добавить логику изменения меша/материалов
        }
    }
}