using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : Item {
 
    public int arrows = 7;

    public Harpoon() : base("Arpón", null, "Arpón usado para cazar pequeños peces en expediciones de submarinismo.", ItemType.Harpoon)
    {
        
    }
  

    private bool Shoot()
    {
        //TODO apply physichs and return succes or not
        arrows--;
        return false;
    }
    private bool PickArrow()
    {
        arrows++;
        return true;
    }
}
