using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Backpack : MonoBehaviour
{
    [System.Serializable]
    public class InventoryEvent : UnityEvent<ItemData, string> { }

    [Header("Config")]
    [SerializeField] private Transform _itemsContainer; // Контейнер для хранения предметов
    [SerializeField] private float _snapDuration = 0.3f; // Время анимации

    public InventoryEvent OnInventoryChanged;

    private Dictionary<ItemType, InventoryItem> _storedItems = new Dictionary<ItemType, InventoryItem>();

    private void Update()
    {
        // Проверка клика мыши на предмет
        DealingWithItemClick();
    }
    public bool TryStoreItem(InventoryItem item)
    {
        if (item == null)
        {
            Debug.LogError("Item is null. Cannot store item in backpack.");
            return false;
        }

        if (item.Data == null)
        {
            Debug.LogError("Item data is null for item: " + item.name);
            return false;
        }

        // Проверка на наличие предмета того же типа в рюкзаке
        if (_storedItems.ContainsKey(item.Data.itemType))
        {
            Debug.LogWarning($"Slot {item.Data.itemType} is occupied!");
            return false;
        }

        item.transform.SetParent(_itemsContainer); // Устанавливаем предмет под контейнер

        Vector3 targetPosition = item.Data.backpackPosition;
        Quaternion targetRotation = Quaternion.Euler(item.Data.backpackRotation);

        StartCoroutine(SnapToPosition(item.transform, targetPosition, targetRotation));
        item.SetInBackpack(); // Устанавливаем статус предмета, что он в рюкзаке

        _storedItems.Add(item.Data.itemType, item);
        OnInventoryChanged?.Invoke(item.Data, "added");
        return true;
    }

    private IEnumerator SnapToPosition(Transform itemTransform, Vector3 targetPosition, Quaternion targetRotation)
    {
        float elapsed = 0f;
        Vector3 startPosition = itemTransform.localPosition;
        Quaternion startRotation = itemTransform.localRotation;

        while (elapsed < _snapDuration)
        {
            itemTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsed / _snapDuration);
            itemTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / _snapDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        itemTransform.localPosition = targetPosition;
        itemTransform.localRotation = targetRotation;
    }
    private void DealingWithItemClick()
    {
        if (Input.GetMouseButtonDown(0)) // Проверка нажатия ЛКМ
        {
            Debug.Log("Checking for item click.");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layerMask = LayerMask.GetMask("Backpack"); // Получаем маску слоя рюкзака, который будем игнорировать
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~layerMask)) // Используем косое отрицание для игнорирования слоя
            {
                Debug.Log($"Hit object: {hit.collider.gameObject.name}"); // выводим имя объекта
                if (hit.collider.TryGetComponent<InventoryItem>(out InventoryItem clickedItem))
                {
                    if (clickedItem.IsInBackpack) // Проверка, предмет в рюкзаке
                    {
                        Debug.Log($"Item clicked: {clickedItem.Data.itemName}. It is in the backpack.");
                        clickedItem.ResetPosition(); // Возвращаем предмет на позицию
                        
                            RemoveItem(clickedItem.Data.itemType);
                            Debug.Log($"Item removed: {clickedItem.Data.itemName}");
                        

                    }
                   
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any object.");
            }
        }
    }

    public void RemoveItem(ItemType itemType)
    {
        if (_storedItems.TryGetValue(itemType, out InventoryItem itemToRemove))
        {
            itemToRemove.Drop(); // Возвращаем предмет
           

            // Удаляем предмет из хранения
            _storedItems.Remove(itemType);
            OnInventoryChanged?.Invoke(itemToRemove.Data, "removed");
        }
    }
}
