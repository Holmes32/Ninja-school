using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]

public class NinjaData
{
    public int Level;
    public int Health;
    public int Damage;
    public float[] position;

    public NinjaData(int Level, int Health,int Damage, Vector2 position)
    {
        this.Level = Level;
        this.Health = Health;
        this.Damage = Damage;
        this.position = new float[2];
        this.position[0] = position.x;
        this.position[1] = position.y;
    }
}
