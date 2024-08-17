using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int x;
    public int y;
    public int enemyNumber;
    public int index;

    public Cell(int _x, int _y, int _enemyNumber, int _index)
    {
        x = _x;
        y = _y;
        enemyNumber = _enemyNumber;
        index = _index;
    }

    public void DisplayInfo()
    {
        Debug.Log("Cell coordination: (" + x + ", " + y + ")" + ", enemyNumber: " + enemyNumber);
    }
}
