using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Numerics;

[System.Diagnostics.DebuggerDisplay("enemies: {enemies}")]

public class CellInfo
{
    public float x, y;
    public List<int> prev_index, next_index;

    public CellInfo(float _x, float _y, List<int> _prev_index, List<int> _next_index)
    {
        x = _x;
        y = _y;
        prev_index = _prev_index;
        next_index = _next_index;
    }
}

public class Stage
{
    public List<Enemy> enemies;
    public List<BigInteger> enemyPassLimits;
    public List<Board> waves;
    public int backGroundNumber;
    
    public Stage(string stageName)
    {
        List<CellInfo> boardInfo = new List<CellInfo>();
        List<int> starts = new List<int>();
        List<int> goals = new List<int>();
        List<List<int>> wavesEnemyInfo = new List<List<int>>();
        switch (stageName)
        {
            case "test":
                //そのステージの情報
                boardInfo = new List<CellInfo>{
                    new CellInfo(-2.0f, 0, new List<int>{}, new List<int>{1}),// (x, y, prev_index, next_index)
                    new CellInfo(-3.2f, 0, new List<int>{0}, new List<int>{2}),
                    new CellInfo(2.0f, 0, new List<int>{1}, new List<int>{3}),
                    new CellInfo(4.0f, 0, new List<int>{2}, new List<int>{4}),
                    new CellInfo(6.0f, 0, new List<int>{3}, new List<int>{}),
                };
                starts = new List<int>{0};
                goals = new List<int>{4};
                //敵の情報
                enemies = new List<Enemy>()
                {
                    new Enemy(10000, new List<int>{1, 2, 3, 4, 5, 6}, 0, 3),// wave1の敵の情報 (enemyNum, Dice, enemyId, enemyの体力)
                    new Enemy(5, new List<int>{1}, 1, 5),// wave2の敵の情報
                };
                wavesEnemyInfo = new List<List<int>>{
                    new List<int>{1, 100, 1000, 10000, 0},//wave1の敵の初期位置 {cell1, cell2, cell3, cell4, cell5}
                    new List<int>{1, 100, 10, 10, 0},
                };
                enemyPassLimits = new List<BigInteger>{1000000, 100000};//敵の通過許容人数
                // 背景の情報
                backGroundNumber = 0;
                break;
            case "test2":
                //そのステージの情報
                boardInfo = new List<CellInfo>{
                    new CellInfo(-2.0f, 0, new List<int>{}, new List<int>{1}),// (x, y, prev_index, next_index)
                    new CellInfo(0, 0, new List<int>{0}, new List<int>{2}),
                    new CellInfo(2.0f, 0, new List<int>{1}, new List<int>{3}),
                    new CellInfo(4.0f, 0, new List<int>{2}, new List<int>{4}),
                    new CellInfo(6.0f, 1.0f, new List<int>{3}, new List<int>{}),
                };
                starts = new List<int>{0};
                goals = new List<int>{4};
                //敵の情報
                enemies = new List<Enemy>()
                {
                    new Enemy(10000, new List<int>{1, 2, 3, 4, 5, 6}, 0, 3),// wave1の敵の情報 (enemyNum, Dice, enemyId, enemyの体力)
                    new Enemy(5, new List<int>{1}, 1, 5),// wave2の敵の情報
                };
                wavesEnemyInfo = new List<List<int>>{
                    new List<int>{1, 100, 1000, 10000, 0},//wave1の敵の初期位置 {cell1, cell2, cell3, cell4, cell5}
                    new List<int>{1, 100, 10, 10, 0},
                };
                enemyPassLimits = new List<BigInteger>{1000000, 100000};//敵の通過許容人数
                // 背景の情報
                backGroundNumber = 1;
                break;
            default:
                enemies = new List<Enemy>();
                waves = new List<Board>();
                enemyPassLimits = new List<BigInteger>();
                break;
        }
        waves = new List<Board>();
        for (int waveIndex = 0; waveIndex < wavesEnemyInfo.Count; waveIndex++)
        {
            Board waveBoard = new Board();
            waveBoard.SetGoal(goals);
            waveBoard.SetStart(starts);
            for (int cellIndex = 0; cellIndex < boardInfo.Count; cellIndex++)
            {
                waveBoard.Add(new Cell(boardInfo[cellIndex].x, boardInfo[cellIndex].y, new Enemy(wavesEnemyInfo[waveIndex][cellIndex], enemies[waveIndex].dice, enemies[waveIndex].id, enemies[waveIndex].turn), cellIndex, boardInfo[cellIndex].prev_index, boardInfo[cellIndex].next_index));
            }
            waves.Add(waveBoard);
        }
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}
