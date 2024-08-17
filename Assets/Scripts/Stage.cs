using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GMTKGameJam.Scripts;

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
        Debug.Log(this);
    }
}