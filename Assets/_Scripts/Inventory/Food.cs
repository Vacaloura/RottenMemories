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

    public AudioClip EatingSound;
    private AudioSource source = null;

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

    public override bool Consume()
    {
        try
        {
            source = GameObject.Find(Names.player).GetComponent<AudioSource>();
        }
        catch (UnityException e) { Debug.Log("No hay AudioSource: " + e.ToString()); }

        switch (this.foodType)
        {
            case FoodType.Roe:
                PlayerController.playerControllerInstance.Eat(roeQuenchValue);
                source.PlayOneShot(EatingSound);
                return true;
            case FoodType.Rabbit:
                PlayerController.playerControllerInstance.Eat(rabbitQuenchValue);
                source.PlayOneShot(EatingSound);
                return true;
            case FoodType.Cat:
                PlayerController.playerControllerInstance.Eat(catQuenchValue);
                source.PlayOneShot(EatingSound);
                return true;
            /*case FoodType.Wine:
                player.eat(wineQuenchValue);
                break;*/
            case FoodType.PreCooked:
                PlayerController.playerControllerInstance.Eat(preCookedQuenchValue);
                source.PlayOneShot(EatingSound);
                return true;
            default:
                Debug.Log("FoodType error: " + foodType);
                return false;
        }
    }
}
