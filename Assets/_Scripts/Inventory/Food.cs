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

    public int roeQuenchValue = 15;
    public int rabbitQuenchValue = 10;
    public int catQuenchValue = 100;
    public int wineQuenchValue = 0;
    public int preeCookedQuenchValue = 5;

    public Food(string name, Sprite icon, string desc, ItemType itype, FoodType ftype) : base(name, icon, desc, itype)
    {
        this.foodType = ftype;
    }
    public Food(string name,string desc, ItemType itype, FoodType ftype) : base(name, desc, itype)
    {
        this.foodType = ftype;
    }

    public void Consume(PlayerController player)
    {
        switch (this.foodType)
        {
            case FoodType.Roe:
                //player.eat(roeQuenchValue);
                break;
            case FoodType.Rabbit:
                //player.eat(rabbitQuenchValue);
                break;
            case FoodType.Cat:
                //player.eat(catQuenchValue);
                break;
            case FoodType.Wine:
                //player.eat(wineQuenchValue);
                break;
            case FoodType.PreCooked:
                //player.eat(preCookedQuenchValue);
                break;
            default:
                Debug.Log("FoodType error: " + foodType);
                break;
        }
    }
}
