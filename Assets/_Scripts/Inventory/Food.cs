using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Food : Item {

    public FoodType foodType = FoodType.NotDefined;
    public enum FoodType
    {
        Roe,
        Rabbit,
        Cat,
        PreCooked,
        NotDefined
    }


    public int roeQuenchValue = 20;
    public int rabbitQuenchValue = 10;
    public int catQuenchValue = 100;
    //public int wineQuenchValue = 0;
    public int preCookedQuenchValue = 5;

    public Food(string name, string iconName, string desc, ItemType itype, FoodType ftype) : base(name, iconName, desc, itype)
    {
        this.foodType = ftype;
    }
    public Food(string name,string desc, ItemType itype, FoodType ftype) : base(name, desc, itype)
    {
        this.foodType = ftype;
    }

    public override bool Consume()
    {

        switch (this.foodType)
        {
            case FoodType.Roe:
                PlayerController.playerControllerInstance.Eat(roeQuenchValue);
                return true;
            case FoodType.Rabbit:
                PlayerController.playerControllerInstance.Eat(rabbitQuenchValue);
                return true;
            case FoodType.Cat:
                PlayerController.playerControllerInstance.PlayerWin("Te has comido a tu gato. Eres un monstruo sin sentimientos.");
                Inventory.inventoryInstance.inventoryPanel.SetActive(false);
                Inventory.inventoryInstance.infoPanel.SetActive(false);
                PlayerController.playerControllerInstance.madness = 99;
                return false;
            /*case FoodType.Wine:
                player.eat(wineQuenchValue);
                break;*/
            case FoodType.PreCooked:
                PlayerController.playerControllerInstance.Eat(preCookedQuenchValue);
                return true;
            default:
                Debug.Log("FoodType error: " + foodType);
                return false;
        }
    }
}
