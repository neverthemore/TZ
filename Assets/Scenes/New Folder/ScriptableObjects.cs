// ItemConfig.cs
using UnityEngine;

[CreateAssetMenu(fileName = "ItemConfig", menuName = "Inventory/Item Config")]
public class ItemConfig : ScriptableObject
{
    public string itemName;
    public int itemID;
    public float weight;
    public ItemType itemType;
    public GameObject prefab;
}

public enum ItemType { Weapon, Potion, Tool }