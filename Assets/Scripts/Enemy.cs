using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[System.Diagnostics.DebuggerDisplay("count: {count}, dice: {dice}")]
public class Enemy
{
    public int count;
    public List<int> dice;
    public int id;

    public Enemy(int _count, List<int> _dice, int _id)
    {
        count = _count;
        dice = _dice;
        id = _id;
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}

public class Enemies {
    
}
