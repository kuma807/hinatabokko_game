using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Numerics;

[System.Diagnostics.DebuggerDisplay("enemies: {enemies}")]
public class Stage
{
    public List<Enemy> enemies;
    public List<Board> waves;
    public List<BigInteger> enemyPassLimits;
    
    public Stage(string stageName)
    {
        switch (stageName)
        {
            case "test":
                enemies = new List<Enemy>()
                {
                    new Enemy(10000, new List<int>{1, 2, 3, 4, 5, 6}, 0, 3),
                    new Enemy(5, new List<int>{1}, 1, 5)
                };
                waves = new List<Board>()
                {
                    new Board{
                        new Cell(-2, 0, new Enemy(1, new List<int>{1, 2, 3, 4, 5, 6}, 0, 5), 0, new List<int>{}, new List<int>{1}),
                        new Cell(0, 0, new Enemy(100, new List<int>{1, 2, 3, 4, 5, 6}, 0, 5), 1, new List<int>{0}, new List<int>{2}),
                        new Cell(2, 0, new Enemy(1000, new List<int>{1, 2, 3, 4, 5, 6}, 0, 5), 2, new List<int>{1}, new List<int>{3}),
                        new Cell(4, 0, new Enemy(10000, new List<int>{1, 2, 3, 4, 5, 6}, 0, 5), 3, new List<int>{2}, new List<int>{4}),
                        new Cell(6, 0, new Enemy(1000000000, new List<int>{1, 2, 3, 4, 5, 6}, 0, 5), 4, new List<int>{3}, new List<int>{}),
                    },
                    new Board{
                        new Cell(-2, 0, new Enemy(1, new List<int>{1, 2, 3, 4, 5, 6}, 1, 5), 0, new List<int>{}, new List<int>{1}),
                        new Cell(0, 0, new Enemy(100, new List<int>{1, 2, 3, 4, 5, 6}, 1, 5), 1, new List<int>{0}, new List<int>{2}),
                        new Cell(2, 0, new Enemy(1000, new List<int>{1, 2, 3, 4, 5, 6}, 1, 5), 2, new List<int>{1}, new List<int>{3}),
                        new Cell(4, 0, new Enemy(10000, new List<int>{1, 2, 3, 4, 5, 6}, 1, 5), 3, new List<int>{2}, new List<int>{4}),
                        new Cell(6, 0, new Enemy(1000000000, new List<int>{1, 2, 3, 4, 5, 6}, 1, 5), 4, new List<int>{3}, new List<int>{}),
                    },
                };
                List<int> goal = new List<int>{5};
                SetGoal(ref waves, goal);
                enemyPassLimits = new List<BigInteger>{100000, 100000};
                break;
            default:
                enemies = new List<Enemy>();
                waves = new List<Board>();
                enemyPassLimits = new List<BigInteger>();
                break;
        }
    }

    public void SetGoal(ref List<Board> waves, List<int> goal)
    {
        for (int i = 0; i < waves.Count; i++)
        {
            waves[i].SetGoal(goal);
        }
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}
