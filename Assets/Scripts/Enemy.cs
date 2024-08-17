using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[System.Diagnostics.DebuggerDisplay("count: {count}, dice: {dice}")]
public class Enemy
{
    public int count;
    public List<int> dice;

    public Enemy(int _count, List<int> _dice)
    {
        count = _count;
        dice = _dice;
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}

public class Enemies {
    
}
