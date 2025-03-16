// Inventory/Backpack.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class Backpack : MonoBehaviour
{
    [System.Serializable]
    public class InventoryEvent : UnityEvent<ItemData, string> { }

    [Header("Configuration")]
    [SerializeField] private Transform _itemsContainer; //  онтейнер дл€ хранени€
    [SerializeField] private float _snapDuration = 0.3f;

    [Header("Events")]
    public InventoryEvent OnInventoryChanged;

    private Dictionary<ItemType, InventoryItem> _storedItems = new();
    private bool _isInventoryVisible;

  

    public void RemoveItem(ItemType type)
    {
        if (!_storedItems.TryGetValue(type, out InventoryItem item)) return;

        // ¬озвращаем оригинального родител€ перед анимацией
        item.transform.SetParent(item.OriginalParent);

        StartCoroutine(SnapToPosition(
            item.transform,
            item.OriginalLocalPosition,
            item.OriginalLocalRotation
        ));

        _storedItems.Remove(type);

        OnInventoryChanged?.Invoke(item.Data, "removed");
        NetworkManager.Instance.SendInventoryEvent(item.Data.itemID, "remove");
    }

    private IEnumerator SnapToPosition(
        Transform itemTransform,
        Vector3 targetLocalPosition,
        Quaternion targetLocalRotation
    )
    {
        float elapsed = 0f;
        Vector3 startPosition = itemTransform.localPosition;
        Quaternion startRotation = itemTransform.localRotation;

        while (elapsed < _snapDuration)
        {
            itemTransform.localPosition = Vector3.Lerp(
                startPosition,
                targetLocalPosition,
                elapsed / _snapDuration
            );

            itemTransform.localRotation = Quaternion.Slerp(
                startRotation,
                targetLocalRotation,
                elapsed / _snapDuration
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        itemTransform.localPosition = targetLocalPosition;
        itemTransform.localRotation = targetLocalRotation;
    }

    private Vector3 GetTargetPosition(ItemData data)
    {
        return data.backpackPosition; // “еперь работаем с локальными координатами
    }

    public bool TryStoreItem(InventoryItem item)
    {
        if (_storedItems.ContainsKey(item.Data.itemType))
        {
            Debug.LogWarning($"Slot {item.Data.itemType} is occupied!");
            return false;
        }

        // ”станавливаем нового родител€ перед анимацией
        item.transform.SetParent(_itemsContainer);

        StartCoroutine(SnapToPosition(
            item.transform,
            item.Data.backpackPosition,
            Quaternion.Euler(item.Data.backpackRotation)
        ));

        _storedItems.Add(item.Data.itemType, item);

        OnInventoryChanged?.Invoke(item.Data, "added");
        NetworkManager.Instance.SendInventoryEvent(item.Data.itemID, "add");
        return true;
    }
}