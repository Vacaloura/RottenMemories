using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZombieData {

    public float[] pos = new float[3];
    public bool isActive;
    public int life;
    public bool firstAttackFlag;

    public ZombieData() { }

    public ZombieData(float[] pos, int life, bool isActive, bool firstAttackFlag)
    {
        this.pos = pos;
        this.life = life;
        this.isActive = isActive;
        this.firstAttackFlag = firstAttackFlag;
    }

    public Vector3 getPos()
    {
        return new Vector3(pos[0], pos[1], pos[2]);
    }
}
