using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Para que cuando lo use se pueda ver en Unity
public class Item
{
    public string itemName = "NewItem";
    public string itemSpriteName = null;
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

    public Item(string name, string iconName, string desc, ItemType type)
    {
        this.itemName = name;
        this.itemSpriteName = iconName;
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

    public virtual bool Consume() {
        if (itemType == ItemType.MakeUp)
        {
            PlayerController.playerControllerInstance.isMadeUp = true;
            PlayerController.playerControllerInstance.avatar.sprite = Resources.Load<Sprite>("sprite_Anxo_normal");
            DisplayManager.displayManagerInstance.DisplayMessage(GameStrings.gameStringsInstance.GetString("MakedUp", null), 2.0f);
            return true;
        }
        return false; }

}
