using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[System.Diagnostics.DebuggerDisplay("cells: {cells}")]
public class Board: List<Cell>
{
    public List<int> start, goal;
    public Board() {}
    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}
