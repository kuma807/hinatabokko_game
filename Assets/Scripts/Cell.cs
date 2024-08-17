using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int x;
    public int y;
    public int enemyNumber;

    public Cell(int _x, int _y, int _enemyNumber)
    {
        x = _x;
        y = _y;
        enemyNumber = _enemyNumber;
    }

    public void DisplayInfo()
    {
        Debug.Log("Cell coordination: (" + x + ", " + y + ")" + ", enemyNumber: " + enemyNumber);
    }
}
