using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[System.Diagnostics.DebuggerDisplay("enemies: {enemies}")]
public class Stage
{
    public List<Enemy> enemies;
    public List<Board> waves;
    
    public Stage(List<Enemy> _enemies, List<Board> _waves)
    {
        enemies = _enemies;
        waves = _waves;
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}
