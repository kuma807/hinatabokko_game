using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[System.Diagnostics.DebuggerDisplay("count: {count}, dice: {dice}")]
public class Enemy
{
    public int count;
    public List<int> dice;
    public int index;

    public Enemy(int _count, List<int> _dice, int _index)
    {
        count = _count;
        dice = _dice;
        index = _index;
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}

public class Enemies {
    
}
