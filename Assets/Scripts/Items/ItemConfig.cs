// Items/ItemData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Tooltip("Unique identifier for the item (must be unique)")]
    public string itemID; // ���������� �������������

    [Tooltip("Display name for the item")]
    public string itemName; // ������������ ���

    [Tooltip("Weight in kilograms")]
    [Range(0.1f, 50f)] public float weight; // ��� � ��

    [Tooltip("Item category type")]
    public ItemType itemType; // ��� ��������

    [Tooltip("Prefab reference with correct transforms")]
    public GameObject physicalPrefab; // ������ ��� 3D �������������

    [Tooltip("UI icon for inventory")]
    public Sprite uiIcon; // ������ ��� UI

    [Tooltip("Local position offset on backpack")]
    public Vector3 backpackPosition; // ������� �� �������

    [Tooltip("Local rotation on backpack")]
    public Vector3 backpackRotation; // ������� �� �������
}

public enum ItemType
{
    Weapon,
    Consumable,
    Tool
}