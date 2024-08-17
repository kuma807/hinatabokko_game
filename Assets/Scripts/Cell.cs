using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int x;
    public int y;
    public int enemyNumber;
    public int index;
    public int[] next_index;
    public int[] prev_index;

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
        UnityEngine.Debug.Log("Cell coordination: (" + x + ", " + y + ")" + ", enemyNumber: " + enemyNumber);
    }
}
