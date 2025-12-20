using System;
using System.Linq.Expressions;
using StarterAssets;
using UnityEngine;

public class ItemsPickuper : MonoBehaviour
{
    [SerializeField] private bool autoPickup = false;
    [SerializeField] private string worldItemTag = "WorldItem";
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private StarterAssetsInputs inputs;

    private bool _canTryPickup = false;
    private WorldItem _pickupWorldItem;

    private void Awake()
    {
        // inputs = new StarterAssetsInputs();
    }

    private void OnEnable()
    {
        inputs.OnPlayerAction += HandlePickup;
        _pickupWorldItem = null;
    }

    private void OnDisable()
    {
        inputs.OnPlayerAction -= HandlePickup;
        _pickupWorldItem = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out _pickupWorldItem))
        {
            _canTryPickup = true;

            if (autoPickup)
                TryPickup();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(worldItemTag))
        {
            _canTryPickup = false;
            _pickupWorldItem = null;
        }
    }

    private void HandlePickup(bool isPickupPressed)
    {
        if (isPickupPressed)
        {
            TryPickup();
        }
    }

    private void TryPickup()
    {
        if (!inputs.player_action)
            return;

        if (!_canTryPickup)
            return;

        if (_pickupWorldItem == null)
        {
            Debug.LogError("Нет активного объекта для добавления в инвентарь!");
            return;
        }

        inventorySystem.AddItem(_pickupWorldItem.ItemData);
        _pickupWorldItem.Pickup();
        _canTryPickup = false;
    }
}
