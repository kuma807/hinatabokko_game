using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Numerics;

public class GameController : MonoBehaviour
{
    
    private Stage stage;
    public int wave_num = 0;
    private List<GameObject> instantiatedEnemies = new List<GameObject>();
    public List<GameObject> enemyObjects;
    public List<Board> waves;
    public Board board;
    public int turn = 0;

    // Start is called before the first frame update
    void Start()
    {
        List<Enemy> enemies = new List<Enemy>()
        {
            new Enemy(10000, new List<int>{1, 2, 3, 4, 5, 6}, 0, 3),
            new Enemy(5, new List<int>{1}, 1, 5)
        };
        waves = new List<Board>()
        {
            new Board{
                new Cell(0, 0, new Enemy(1, new List<int>{1, 2, 3, 4, 5, 6}, 0, 5), 0),
                new Cell(2, 0, new Enemy(100, new List<int>{1, 2, 3, 4, 5, 6}, 0, 5), 1),
                new Cell(4, 0, new Enemy(1000, new List<int>{1, 2, 3, 4, 5, 6}, 0, 5), 2),
                new Cell(6, 0, new Enemy(10000, new List<int>{1, 2, 3, 4, 5, 6}, 0, 5), 3),
                new Cell(8, 0, new Enemy(1000000000, new List<int>{1, 2, 3, 4, 5, 6}, 0, 5), 4),
            },
            new Board{
                new Cell(0, 0, new Enemy(1, new List<int>{1, 2, 3, 4, 5, 6}, 1, 5), 0),
                new Cell(2, 0, new Enemy(100, new List<int>{1, 2, 3, 4, 5, 6}, 1, 5), 1),
                new Cell(4, 0, new Enemy(1000, new List<int>{1, 2, 3, 4, 5, 6}, 1, 5), 2),
                new Cell(6, 0, new Enemy(10000, new List<int>{1, 2, 3, 4, 5, 6}, 1, 5), 3),
                new Cell(8, 0, new Enemy(1000000000, new List<int>{1, 2, 3, 4, 5, 6}, 1, 5), 4),
            },
        };
        stage = new Stage(enemies, waves);
        board = stage.waves[wave_num];
        DrawEnemy(ref board);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UpdateTurn();
            if (wave_num < stage.enemies.Count && turn == stage.enemies[wave_num].turn)
            {
                turn = 0;
                wave_num += 1;
                if (wave_num >= stage.waves.Count)
                {
                    board = new Board();
                } 
                else
                {
                    board = stage.waves[wave_num];
                }
                
                DrawEnemy(ref board);
            }
        }
    }

    void UpdateTurn()
    {
        MoveEnemies(ref board);
        DrawEnemy(ref board);
        turn += 1;
    }

    // 将来的にはEffectとかでこの処理を行うべき
    void MoveEnemies(ref Board board)
    {
        if (board.Count >= 2) 
        {
            board[board.Count - 1].enemy.count += board[board.Count - 2].enemy.count;
        }
        for (int i = board.Count - 3; i >= 0; i--)
        {
            board[i + 1].enemy.count = board[i].enemy.count;
        }
        if (board.Count != 0)
        {
            board[0].enemy = new Enemy(0, stage.enemies[wave_num].dice, stage.enemies[wave_num].id, 5);   
        }
        string outputString = "Board";
        for (int i = 0; i < board.Count; i++)
        {
            outputString += " " + board[i].enemy.count + ",";
        }
        Debug.Log(outputString);
    }

    void DrawEnemy(ref Board board)
    {
        foreach (GameObject enemyInstance in instantiatedEnemies)
        {
            if (enemyInstance != null)
            {
                Destroy(enemyInstance);
            }
        }
        instantiatedEnemies.Clear();
        for (int i = 0; i < board.Count; i++)
        {
            if (board[i].enemy.count != 0)
            {
                float scale = (float)2.0 + (float)Math.Log((double)board[i].enemy.count, 10.0) / (float)2.0;
                float enemyHight = enemyObjects[board[i].enemy.id].GetComponent<SpriteRenderer>().bounds.size.y;
                GameObject enemyInstance = Instantiate(enemyObjects[board[i].enemy.id], new UnityEngine.Vector3(board[i].x, board[i].y + (float)(enemyHight * scale / 2.0) - (float)0.4, 0), UnityEngine.Quaternion.identity);
                enemyInstance.transform.localScale = new UnityEngine.Vector3(scale, scale, scale);
                instantiatedEnemies.Add(enemyInstance);
            }
        }
    }
}
