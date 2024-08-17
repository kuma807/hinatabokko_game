using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[System.Diagnostics.DebuggerDisplay("ememies: {ememies}")]
public class Stage
{
    public Enemy[] ememies;
    public Board board;

    public Stage(Enemy[] _ememies, Board _board)
    {
        ememies = _ememies;
        board = _board;
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}
