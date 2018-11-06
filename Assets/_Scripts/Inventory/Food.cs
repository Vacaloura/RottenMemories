using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item {

    public FoodType foodType = FoodType.NotDefined;
    public enum FoodType
    {
        Roe,
        Rabbit,
        Cat,
        Wine,
        PreCooked,
        NotDefined
    }

    public int roeQuenchValue = 20;
    public int rabbitQuenchValue = 10;
    public int catQuenchValue = 100;
    //public int wineQuenchValue = 0;
    public int preCookedQuenchValue = 5;

    public Food(string name, Sprite icon, string desc, ItemType itype, FoodType ftype) : base(name, icon, desc, itype)
    {
        this.foodType = ftype;
    }
    public Food(string name,string desc, ItemType itype, FoodType ftype) : base(name, desc, itype)
    {
        this.foodType = ftype;
    }

    public override bool Consume(PlayerController player)
    {
        switch (this.foodType)
        {
            case FoodType.Roe:
                player.Eat(roeQuenchValue);
                return true;
            case FoodType.Rabbit:
                player.Eat(rabbitQuenchValue);
                return true;
            case FoodType.Cat:
                player.Eat(catQuenchValue);
                return true;
            /*case FoodType.Wine:
                player.eat(wineQuenchValue);
                break;*/
            case FoodType.PreCooked:
                player.Eat(preCookedQuenchValue);
                return true;
            default:
                Debug.Log("FoodType error: " + foodType);
                return false;
        }
    }
}
