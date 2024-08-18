using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;


[System.Diagnostics.DebuggerDisplay("count: {count}, dice: {dice}")]
public class Enemy
{
    public BigInteger count;
    public List<int> dice;
    public int id;
    public BigInteger turn;

    public Enemy(BigInteger _count, List<int> _dice, int _id, BigInteger _turn)
    {
        count = _count;
        dice = _dice;
        id = _id;
        turn = _turn;
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}

public class Enemies {
    
}
