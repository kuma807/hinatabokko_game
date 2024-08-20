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
    public Inventory inventory;
    
    public Stage(string stageName)
    {
        List<CellInfo> boardInfo = new List<CellInfo>();
        List<int> starts = new List<int>();
        List<int> goals = new List<int>();
        List<int> unchangeable = new List<int>();
        List<List<BigInteger>> wavesEnemyInfo = new List<List<BigInteger>>();
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
                unchangeable = new List<int>{4};
                //敵の情報
                enemies = new List<Enemy>()
                {
                    new Enemy(10000, new List<int>{1, 2, 3, 4, 5, 6}, 0, 3),// wave1の敵の情報 (enemyNum, Dice, enemyId, enemyの体力)
                    new Enemy(5, new List<int>{1}, 1, 5),// wave2の敵の情報
                };
                wavesEnemyInfo = new List<List<BigInteger>>{
                    new List<BigInteger>{1, 100, 1000, 10000, 0},//wave1の敵の初期位置 {cell1, cell2, cell3, cell4, cell5}
                    new List<BigInteger>{1, 100, 10, 10, 0},
                };
                enemyPassLimits = new List<BigInteger>{1000000, 100000};//敵の通過許容人数
                // 背景の情報
                backGroundNumber = 0;
                break;
            case "Tutorial":
                //そのステージの情報
                int sizet = 10;
                boardInfo = new List<CellInfo>(sizet);
                for (int i = 0; i < sizet; i++)
                {
                    var prev_index = new List<int>();
                    var next_index = new List<int>();
                    if (i > 0) prev_index.Add(i - 1);
                    if (i + 1 < sizet) next_index.Add(i + 1);
                    if (((i / 10) & 1) == 0) boardInfo.Add(new CellInfo(-7f + (1.5f * (i % 10)), 2.5f - (i / 10) * 1.5f, prev_index, next_index));
                    else boardInfo.Add(new CellInfo(-7f + (1.5f * (9 - i % 10)), 2.5f - (i / 10) * 1.5f, prev_index, next_index));
                }
                starts = new List<int>{0};
                goals = new List<int>{sizet - 1};
                unchangeable = new List<int>{0, sizet - 1};
                //敵の情報
                BigInteger enemy1countt = GameCalculater.TEN(1);
                enemies = new List<Enemy>()
                {
                    new Enemy(enemy1countt, new List<int>{1, 2, 3}, 0, 3),// wave1の敵の情報 (enemyNum, Dice, enemyId, enemyの体力)
                    // new Enemy(enemy2count1, new List<int>{1, 3, 5}, 1, 5),// wave2の敵の情報
                };
                wavesEnemyInfo = new List<List<BigInteger>>(enemies.Count);
                for (int i = 0; i < enemies.Count; i++)
                {
                    wavesEnemyInfo.Add(new List<BigInteger>(sizet));
                    for (int j = 0; j < sizet; j++) wavesEnemyInfo[i].Add(0);
                };
                wavesEnemyInfo[0][0] = enemy1countt;
                // wavesEnemyInfo[1][0] = enemy2count1;
                enemyPassLimits = new List<BigInteger>{5};//敵の通過許容人数
                // 背景の情報
                backGroundNumber = 0;
                inventory = Inventory.TutorialInventory();
                break;
            case "Stage1":
                //そのステージの情報
                int size1 = 20;
                boardInfo = new List<CellInfo>(size1);
                for (int i = 0; i < size1; i++)
                {
                    var prev_index = new List<int>();
                    var next_index = new List<int>();
                    if (i > 0) prev_index.Add(i - 1);
                    if (i + 1 < size1) next_index.Add(i + 1);
                    if (((i / 10) & 1) == 0) boardInfo.Add(new CellInfo(-7f + (1.5f * (i % 10)), 2.5f - (i / 10) * 1.5f, prev_index, next_index));
                    else boardInfo.Add(new CellInfo(-7f + (1.5f * (9 - i % 10)), 2.5f - (i / 10) * 1.5f, prev_index, next_index));
                }
                boardInfo[6].next_index.Add(14);
                starts = new List<int>{0, 1};
                goals = new List<int>{size1 - 2, size1 - 1};
                unchangeable = new List<int>{0, 1, size1 - 1, size1 - 2};
                //敵の情報
                BigInteger enemy1count1 = GameCalculater.TEN(5);
                BigInteger enemy2count1 = GameCalculater.TEN(6);
                enemies = new List<Enemy>()
                {
                    new Enemy(enemy1count1, new List<int>{1, 2, 3, 4, 5, 6}, 0, 3),// wave1の敵の情報 (enemyNum, Dice, enemyId, enemyの体力)
                    new Enemy(enemy2count1, new List<int>{1, 3, 5}, 1, 5),// wave2の敵の情報
                };
                wavesEnemyInfo = new List<List<BigInteger>>(enemies.Count);
                for (int i = 0; i < enemies.Count; i++)
                {
                    wavesEnemyInfo.Add(new List<BigInteger>(size1));
                    for (int j = 0; j < size1; j++) wavesEnemyInfo[i].Add(0);
                };
                wavesEnemyInfo[0][0] = enemy1count1;
                wavesEnemyInfo[1][0] = enemy2count1;
                enemyPassLimits = new List<BigInteger>{enemy1count1 / 20, enemy2count1 / 20};//敵の通過許容人数
                // 背景の情報
                backGroundNumber = 1;
                inventory = Inventory.Stage1Inventory();
                break;
            case "Stage2":
                //そのステージの情報
                int size2 = 20;
                boardInfo = new List<CellInfo>(size2);
                for (int i = 0; i < size2; i++)
                {
                    var prev_index = new List<int>();
                    var next_index = new List<int>();
                    if (i > 0) prev_index.Add(i - 1);
                    if (i + 1 < size2) next_index.Add(i + 1);
                    if (((i / 10) & 1) == 0) boardInfo.Add(new CellInfo(-7f + (1.5f * (i % 10)), 2.5f - (i / 10) * 1.5f, prev_index, next_index));
                    else boardInfo.Add(new CellInfo(-7f + (1.5f * (9 - i % 10)), 2.5f - (i / 10) * 1.5f, prev_index, next_index));
                }
                starts = new List<int>{0};
                goals = new List<int>{size2 - 1};
                unchangeable = new List<int>{size2 - 1};
                //敵の情報
                BigInteger enemy1count2 = GameCalculater.TEN(5);
                BigInteger enemy2count2 = GameCalculater.TEN(6);
                enemies = new List<Enemy>()
                {
                    new Enemy(enemy1count1, new List<int>{1, 2, 3, 4, 5, 6}, 0, 3),// wave1の敵の情報 (enemyNum, Dice, enemyId, enemyの体力)
                    new Enemy(enemy2count1, new List<int>{1, 3, 5}, 1, 5),// wave2の敵の情報
                };
                wavesEnemyInfo = new List<List<BigInteger>>(enemies.Count);
                for (int i = 0; i < enemies.Count; i++)
                {
                    wavesEnemyInfo.Add(new List<BigInteger>(size2));
                    for (int j = 0; j < size2; j++) wavesEnemyInfo[i].Add(0);
                };
                wavesEnemyInfo[0][0] = enemy1count2;
                wavesEnemyInfo[1][0] = enemy2count2;
                enemyPassLimits = new List<BigInteger>{enemy1count2 / 20, enemy2count2 / 20};//敵の通過許容人数
                // 背景の情報
                backGroundNumber = 1;
                inventory = Inventory.Stage2Inventory();
                break;
            case "SomeoneStage":
                //そのステージの情報
                int sizes = 30;
                boardInfo = new List<CellInfo>(sizes);
                for (int i = 0; i < sizes; i++)
                {
                    var prev_index = new List<int>();
                    var next_index = new List<int>();
                    if (i > 0) prev_index.Add(i - 1);
                    if (i + 1 < sizes) next_index.Add(i + 1);
                    if (((i / 10) & 1) == 0) boardInfo.Add(new CellInfo(-7f + (1.5f * (i % 10)), 2.5f - (i / 10) * 1.5f, prev_index, next_index));
                    else boardInfo.Add(new CellInfo(-7f + (1.5f * (9 - i % 10)), 2.5f - (i / 10) * 1.5f, prev_index, next_index));
                }
                boardInfo[8].next_index.Add(15);
                boardInfo[4].next_index.Add(11);
                boardInfo[13].next_index.Add(22);
                boardInfo[19].next_index.Add(21);
                boardInfo[8].next_index.Add(23);
                starts = new List<int>{0};
                goals = new List<int>{sizes - 1};
                unchangeable = new List<int>{sizes - 1};
                //敵の情報
                BigInteger enemy1counts = GameCalculater.TEN(15);
                BigInteger enemy2counts = GameCalculater.TEN(20);
                enemies = new List<Enemy>()
                {
                    new Enemy(enemy1count1, new List<int>{2, 3, 5, 7}, 0, GameCalculater.TEN(15)),// wave1の敵の情報 (enemyNum, Dice, enemyId, enemyの体力)
                    new Enemy(enemy2count1, new List<int>{1, 3, 4}, 1, GameCalculater.TEN(20)),// wave2の敵の情報
                };
                wavesEnemyInfo = new List<List<BigInteger>>(enemies.Count);
                for (int i = 0; i < enemies.Count; i++)
                {
                    wavesEnemyInfo.Add(new List<BigInteger>(sizes));
                    for (int j = 0; j < sizes; j++) wavesEnemyInfo[i].Add(0);
                };
                wavesEnemyInfo[0][0] = enemy1counts;
                wavesEnemyInfo[1][0] = enemy2counts;
                enemyPassLimits = new List<BigInteger>{enemy1counts / 10, enemy2counts / 10};//敵の通過許容人数
                // 背景の情報
                backGroundNumber = 0;
                inventory = Inventory.StageSomeoneInventory();
                break;
			case "KaikeyStage":
				//そのステージの情報
				int sizeExt = 20;
				boardInfo = new List<CellInfo>(sizeExt);

				List<List<int>> edges = new List<List<int>>();
				edges.Add(new List<int> { 0, 2 });
				edges.Add(new List<int> { 1, 3 });
				edges.Add(new List<int> { 2, 4 });
				edges.Add(new List<int> { 3, 4 });
				edges.Add(new List<int> { 4, 5 });
				edges.Add(new List<int> { 4, 6 });
				edges.Add(new List<int> { 5, 7 });
				edges.Add(new List<int> { 5, 8 });
				edges.Add(new List<int> { 6, 7 });
				edges.Add(new List<int> { 7, 9 });
				edges.Add(new List<int> { 8, 9 });
				edges.Add(new List<int> { 8, 10 });
				edges.Add(new List<int> { 9, 12 });
				edges.Add(new List<int> { 10, 11 });
				edges.Add(new List<int> { 10, 12 });
				edges.Add(new List<int> { 11, 15 });
				edges.Add(new List<int> { 11, 16 });
				edges.Add(new List<int> { 12, 13 });
				edges.Add(new List<int> { 12, 16 });
				edges.Add(new List<int> { 13, 14 });
				edges.Add(new List<int> { 11, 15 });
				edges.Add(new List<int> { 11, 16 });
				edges.Add(new List<int> { 15, 17 });
				edges.Add(new List<int> { 16, 17 });
				edges.Add(new List<int> { 17, 18 });
				edges.Add(new List<int> { 17, 19 });

				List<List<int>> prevs = new List<List<int>>();
				List<List<int>> nexts = new List<List<int>>();
				for (int i = 0; i < sizeExt; i++)
				{
					prevs.Add(new List<int>());
					nexts.Add(new List<int>());
				}

				foreach (List<int> lst in edges)
				{
					prevs[lst[0]].Add(lst[1]);
					nexts[lst[1]].Add(lst[0]);
				}

				boardInfo.Add(new CellInfo(-8f, 3f, prevs[0], nexts[0]));
				boardInfo.Add(new CellInfo(-8f, -3f, prevs[1], nexts[1]));
				boardInfo.Add(new CellInfo(-6.2f, 2f, prevs[2], nexts[2]));
				boardInfo.Add(new CellInfo(-6.2f, -2f, prevs[3], nexts[3]));
				boardInfo.Add(new CellInfo(-4.4f, 0f, prevs[4], nexts[4]));
				boardInfo.Add(new CellInfo(-2.6f, 1f, prevs[5], nexts[5]));
				boardInfo.Add(new CellInfo(-2.6f, -3f, prevs[6], nexts[6]));
				boardInfo.Add(new CellInfo(-0.8f, -1f, prevs[7], nexts[7]));
				boardInfo.Add(new CellInfo(-0.8f, 2f, prevs[8], nexts[8]));
				boardInfo.Add(new CellInfo(1f, 0f, prevs[9], nexts[9]));
				boardInfo.Add(new CellInfo(1f, 3f, prevs[10], nexts[10]));
				boardInfo.Add(new CellInfo(2.8f, 4f, prevs[11], nexts[11]));
				boardInfo.Add(new CellInfo(2.8f, 1f, prevs[12], nexts[12]));
				boardInfo.Add(new CellInfo(2.8f, -2f, prevs[13], nexts[13]));
				boardInfo.Add(new CellInfo(4.6f, -4f, prevs[14], nexts[14]));
				boardInfo.Add(new CellInfo(4.6f, 3f, prevs[15], nexts[15]));
				boardInfo.Add(new CellInfo(4.6f, 1f, prevs[16], nexts[16]));
				boardInfo.Add(new CellInfo(6.4f, 2f, prevs[17], nexts[17]));
				boardInfo.Add(new CellInfo(8.2f, 4f, prevs[18], nexts[18]));
				boardInfo.Add(new CellInfo(8.2f, 0f, prevs[19], nexts[19]));

				starts = new List<int> { 0, 1 };
				goals = new List<int> { 14, 18, 19 };
				unchangeable = new List<int> { 0, 1, 14, 18, 19 };
				//敵の情報
				BigInteger enemy1count = GameCalculater.TEN(3);
				BigInteger enemy2count = GameCalculater.TEN(6);
				BigInteger enemy3count = GameCalculater.TEN(7);
				enemies = new List<Enemy>()
				{
					new Enemy(enemy1count, new List<int>{1, 2, 3, 4}, 0, 3),// wave1の敵の情報 (enemyNum, Dice, enemyId, enemyの体力)
                    new Enemy(enemy2count, new List<int>{1, 3, 5}, 1, 4),// wave2の敵の情報
                    new Enemy(enemy3count, new List<int>{1, 2, 4, 6}, 2, 7),// wave2の敵の情報
                };
				wavesEnemyInfo = new List<List<BigInteger>>(enemies.Count);
				for (int i = 0; i < enemies.Count; i++)
				{
					wavesEnemyInfo.Add(new List<BigInteger>(sizeExt));
					for (int j = 0; j < sizeExt; j++) wavesEnemyInfo[i].Add(0);
				};
				wavesEnemyInfo[0][0] = enemy1count / 2;
				wavesEnemyInfo[0][1] = enemy1count / 2;
				wavesEnemyInfo[1][0] = enemy2count / 2;
				wavesEnemyInfo[1][1] = enemy2count / 2;
				wavesEnemyInfo[2][0] = enemy3count / 2;
				wavesEnemyInfo[2][1] = enemy3count / 2;
				enemyPassLimits = new List<BigInteger> { enemy1count / 20, enemy2count / 10, enemy3count / 5 };//敵の通過許容人数
																											   // 背景の情報
				backGroundNumber = 1;
				inventory = Inventory.StageKaikeyInventory();
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
            waveBoard.SetUnchangealbe(unchangeable);
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
