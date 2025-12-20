using System;
using StarterAssets;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] private StarterAssetsInputs inputs;
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private string triggerTag = "InerectiveDoor";

    private bool _canOpen = false;
    private OpenDoor _interactiveDoor;

    private void OnEnable()
    {
        inputs.OnPlayerAction += HandleOpen;
    }

    private void OnDisable()
    {
        inputs.OnPlayerAction -= HandleOpen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out _interactiveDoor))
        {
            _canOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            _canOpen = false;
        }
    }

    private void HandleOpen(bool obj)
    {
        TryOpen();
    }

    private void TryOpen()
    {
        if (!_canOpen || _interactiveDoor == null)
        {
            //Debug.LogError("Нет активной двери для открытия!");
            return;
        }

        ItemSO keyItem = inventorySystem.GetUsableItemOfType(ItemType.Key);
        if (keyItem == null)
        {
            Debug.Log("У вас нет ключевого предмета для открытия двери.");
            return;
        }

        if (_interactiveDoor.TryOpen(keyItem))
        {
            inventorySystem.RemoveItem(keyItem);
        }
    }
}