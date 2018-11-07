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
        else if(transform.tag == "Food")
        {
            Inventory.inventoryInstance.AddItem(new Food(transform.name, "Comida que te ayudará a mantenerte cuerdo.", Item.ItemType.Food, Food.FoodType.Roe)); //TODO tipo de comida dinámico
            gameObject.SetActive(false);
        }
        else
        {
            Inventory.inventoryInstance.AddItem(new Item(transform.name, "TODO", Item.ItemType.NotDefined));
            gameObject.SetActive(false);
        }
        GameObject.Find(Names.managers).GetComponent<DisplayManager>().interactText.SetActive(false);
    }
}
