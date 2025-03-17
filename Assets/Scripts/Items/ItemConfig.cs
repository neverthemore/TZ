// Items/ItemData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Tooltip("Unique identifier for the item (must be unique)")]
    public string itemID; // Уникальный идентификатор

    [Tooltip("Display name for the item")]
    public string itemName; // Отображаемое имя

    [Tooltip("Weight in kilograms")]
    [Range(0.1f, 50f)] public float weight; // Вес в кг

    [Tooltip("Item category type")]
    public ItemType itemType; // Тип предмета

    [Tooltip("Prefab reference with correct transforms")]
    public GameObject physicalPrefab; // Префаб для 3D представления

    [Tooltip("UI icon for inventory")]
    public Sprite uiIcon; // Иконка для UI

    [Tooltip("Local position offset on backpack")]
    public Vector3 backpackPosition; // Позиция на рюкзаке

    [Tooltip("Local rotation on backpack")]
    public Vector3 backpackRotation; // Поворот на рюкзаке
}

public enum ItemType
{
    Weapon,
    Consumable,
    Tool
}