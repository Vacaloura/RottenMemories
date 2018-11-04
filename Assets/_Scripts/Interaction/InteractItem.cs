using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItem : Interactable {

    public override void Interact() {
        base.Interact();
        if (transform.tag == "Arrow")
        {
            ((Harpoon)Inventory.inventoryInstance.itemList[0]).arrows++;
            Destroy(this.gameObject);
        }
        else
        {
            Inventory.inventoryInstance.AddItem(new Item(transform.name, "TODO", Item.ItemType.NotDefined));
            gameObject.SetActive(false);
        }
    }
}
