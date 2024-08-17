using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[System.Diagnostics.DebuggerDisplay("enemies: {enemies}")]
public class Stage
{
    public List<Enemy> enemies;
    public Board board;

    public Stage(List<Enemy> _enemies, Board _board)
    {
        enemies = _enemies;
        board = _board;
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}
