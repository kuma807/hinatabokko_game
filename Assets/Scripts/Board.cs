using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GMTKGameJam.Scripts;

[System.Diagnostics.DebuggerDisplay("cells: {cells}")]
public class Board
{
    public Cell[] cells;

    public Board(Cell[] _cells)
    {
        cells = _cells;
    }

    public void DisplayInfo()
    {
        Debug.Log(this);
    }
}
