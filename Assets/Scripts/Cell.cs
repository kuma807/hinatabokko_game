using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTKGameJam.Scripts;

public class Cell
{
    public int x;
    public int y;
    public int enemyNumber;
    public int index;
    int[] next_index;
    int[] prev_index;

    public Cell(int _x, int _y, int _enemyNumber, int _index)
    {
        x = _x;
        y = _y;
        enemyNumber = _enemyNumber;
        index = _index;
        // calculate next_index, prev_index
    }

    public void DisplayInfo()
    {
        Debug.Log("Cell coordination: (" + x + ", " + y + ")" + ", enemyNumber: " + enemyNumber);
    }
}