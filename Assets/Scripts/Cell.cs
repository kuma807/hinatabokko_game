using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int x;
    public int y;
    public List<Enemy> enemies;
    public int index;
    public List<int> next_index;
    public List<int> prev_index;

    public Cell(int _x, int _y, List<Enemy> _enemies, int _index)
    {
        x = _x;
        y = _y;
        enemies = _enemies;
        index = _index;
        // calculate next_index, prev_index
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log("Cell coordination: (" + x + ", " + y + ")");
        foreach (Enemy enemy in enemies) 
        {
            enemy.DisplayInfo();
        }
    }
}
