using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Harpoon : Item {
 
    public int arrows = 7;

    public Harpoon() : base(GameStrings.gameStringsInstance.GetString("HarpoonName", null), "Icono_Harpoon", GameStrings.gameStringsInstance.GetString("HarpoonDescription", null), ItemType.Harpoon)
    {
        
    }
  

    private bool Shoot()
    {
        arrows--;
        return false;
    }
    private bool PickArrow()
    {
        arrows++;
        return true;
    }
}
