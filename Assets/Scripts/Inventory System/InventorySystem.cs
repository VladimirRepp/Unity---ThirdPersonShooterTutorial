using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Хранит и управляет инвентарем игрока
/// </summary>
public class InventorySystem : MonoBehaviour
{
    private List<ItemSO> items = new List<ItemSO>();

    public void AddItem(ItemSO item)
    {
        items.Add(item);
        Debug.Log($"Предмет {item.itemName} добавлен в инвентарь.");
    }

    public void RemoveItem(ItemSO item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"Предмет {item.itemName} удален из инвентаря.");
        }
        else
        {
            Debug.LogWarning($"Попытка удалить предмет {item.itemName}, которого нет в инвентаре.");
        }
    }

    public ItemSO GetUsableItemOfType(ItemType itemType)
    {
        foreach (var item in items)
        {
            if (item.itemType == itemType && item.isUsable)
            {
                return item;
            }
        }
        return null;
    }

    public void DropItem(ItemSO item, Vector3 position, Quaternion rotation)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            item.Drop(position, rotation);
            Debug.Log($"Предмет {item.itemName} выброшен из инвентаря.");
        }
        else
        {
            Debug.LogWarning($"Попытка выбросить предмет {item.itemName}, которого нет в инвентаре.");
        }
    }

    public void ShowInventory()
    {
        Debug.Log("Содержимое инвентаря:");
        foreach (var item in items)
        {
            Debug.Log($"- {item.itemName} (ID: {item.itemID})");
        }
    }
}
