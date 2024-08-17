using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GMTKGameJam.Scripts;

[System.Diagnostics.DebuggerDisplay("count: {count}, dice: {dice}")]
public class Enemy
{
    public int count;
    public int[] dice;

    public Enemy(int _count, int[] _dice)
    {
        count = _count;
        dice = _dice;
    }

    public void DisplayInfo()
    {
        Debug.Log(this);
    }
}

public class Enemies {
    
}
