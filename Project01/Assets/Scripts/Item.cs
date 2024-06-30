using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Weapon,
        Item
    }

    public string itemName;
    public ItemType itemType;
    public int Value;
    public Sprite itemImage;
    public GameObject itemPrefab;

    public override bool Equals(object obj)
    {
        if (obj is Item item)
        {
            return itemName == item.itemName && itemType == item.itemType && Value == item.Value;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (itemName, itemType, Value).GetHashCode();
    }
}