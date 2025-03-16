// Backpack.cs
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Collections;

public class Backpack : MonoBehaviour
{
    [System.Serializable]
    public class Slot
    {
        public ItemType type;
        public Transform uiSlot;
        public Transform modelSlot;
    }

    [SerializeField] private List<Slot> slots = new();
    private Dictionary<ItemType, Transform> itemSlots = new();
    public UnityEvent<Item> OnItemAdded = new();
    public UnityEvent<Item> OnItemRemoved = new();

    private void Start()
    {
        foreach (var slot in slots)
            itemSlots.Add(slot.type, slot.modelSlot);
    }

    public void AddItem(Item item)
    {
        StartCoroutine(SnapToPosition(item.transform, itemSlots[item.config.itemType]));
        OnItemAdded.Invoke(item);
        NetworkManager.Instance.SendItemUpdate(item.config.itemID, "add");
    }

    public void RemoveItem(Item item)
    {
        StartCoroutine(SnapToPosition(item.transform, null)); // Вернуть в мир
        OnItemRemoved.Invoke(item);
        NetworkManager.Instance.SendItemUpdate(item.config.itemID, "remove");
    }

    private IEnumerator SnapToPosition(Transform item, Transform target)
    {
        float duration = 0.5f;
        float elapsed = 0;
        Vector3 startPos = item.position;

        while (elapsed < duration)
        {
            item.position = Vector3.Lerp(startPos, target.position, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}