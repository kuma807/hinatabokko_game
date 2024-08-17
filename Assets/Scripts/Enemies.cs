using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[System.Diagnostics.DebuggerDisplay("count: {count}, dice: {dice}")]
public class Enemies
{
    public int count;
    public int[] dice;

    public Enemies(int _count, int[] _dice)
    {
        count = _count;
        dice = _dice;
    }

    public void DisplayInfo()
    {
        Debug.Log(this);
    }
}
