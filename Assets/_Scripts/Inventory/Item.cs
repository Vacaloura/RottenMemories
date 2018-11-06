using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Para que cuando lo use se pueda ver en Unity
public class Item
{
    public string itemName = "NewItem";
    public Sprite itemSprite = null;
    public string itemDescription = "Description of the item";
    public ItemType itemType = ItemType.NotDefined;
    public enum ItemType
    {
        Food,
        Diary,
        Harpoon,
        WineBottle,
        MakeUp,
        Ladder,
        NotDefined
    }

    public int itemAmount = 1;

    public Item(string name, Sprite icon, string desc, ItemType type)
    {
        this.itemName = name;
        this.itemSprite = icon;
        this.itemDescription = desc;
        this.itemType = type;
    }

    public Item(string name, string desc, ItemType type)
    {
        this.itemName = name;
        this.itemDescription = desc;
        this.itemType = type;
    }

    public void Increase()
    {
        itemAmount++;
    }

    public virtual bool Consume(PlayerController player) { return false; }

}
