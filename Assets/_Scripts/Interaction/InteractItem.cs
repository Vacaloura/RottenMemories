using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItem : Interactable {

    public override void Interact() {
        base.Interact();
        Inventory.inventoryInstance.AddItem(new Item("Esfera", "Esfera de ejemplo", Item.ItemType.NotDefined));
        gameObject.SetActive(false);
    }
}
