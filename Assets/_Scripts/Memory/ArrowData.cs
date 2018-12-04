using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArrowData{

    public float[] pos = new float[3];
    public string parentName;
    public bool isActive;
    public bool isKinematic;

    public ArrowData() { }

    public ArrowData(float[] pos, string parentName, bool isActive, bool isKinematic)
    {
        this.pos = pos;
        this.parentName = parentName;
        this.isActive = isActive;
        this.isKinematic = isKinematic;
    }

    public Vector3 getPos()
    {
        return new Vector3(pos[0], pos[1], pos[2]);
    }
}
