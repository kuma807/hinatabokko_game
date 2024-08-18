using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class GameController : MonoBehaviour
{
    public GameObject[] enemyObjects;
    private Stage stage;
    private List<GameObject> instantiatedEnemies = new List<GameObject>();
    public int wave = 0;

    // Start is called before the first frame update
    void Start()
    {
        List<Enemy> enemies = new List<Enemy>()
        {
            new Enemy(1, new List<int>{1, 2, 3, 4, 5, 6}, 0),
            new Enemy(2, new List<int>{1}, 1)
        };
        Board board = new Board()
        {
            new Cell(0, 0, new Enemy(1, new List<int>{1, 2, 3, 4, 5, 6}, 0), 0, new NullStepOnEffect(), new NullRollDiceEffect()),
            new Cell(2, 0, new Enemy(100, new List<int>{1, 2, 3, 4, 5, 6}, 1), 1, new NullStepOnEffect(), new NullRollDiceEffect()),
            new Cell(4, 0, new Enemy(1000, new List<int>{1, 2, 3, 4, 5, 6}, 0), 2, new NullStepOnEffect(), new NullRollDiceEffect()),
            new Cell(6, 0, new Enemy(10000, new List<int>{1, 2, 3, 4, 5, 6}, 0), 3, new NullStepOnEffect(), new NullRollDiceEffect()),
            new Cell(8, 0, new Enemy(1000000000, new List<int>{1, 2, 3, 4, 5, 6}, 0), 4, new NullStepOnEffect(), new NullRollDiceEffect()),
        };
        stage = new Stage(enemies, board);
        DrawEnemy(ref stage.board);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UpdateTurn();
        }
    }

    void UpdateTurn()
    {
        MoveEnemies(ref stage.board);
        DrawEnemy(ref stage.board);
    }

    // 将来的にはEffectとかでこの処理を行うべき
    void MoveEnemies(ref Board board)
    {
        board[board.Count - 1].enemy.count += board[board.Count - 2].enemy.count;
        for (int i = board.Count - 3; i >= 0; i--)
        {
            board[i + 1].enemy.count = board[i].enemy.count;
        }
        board[0].enemy = new Enemy(0, stage.enemies[wave].dice, stage.enemies[wave].id);
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
                GameObject enemyInstance = Instantiate(enemyObjects[board[i].enemy.id], new Vector3(board[i].x, board[i].y + (float)(enemyHight * scale / 2.0) - (float)0.4, 0), Quaternion.identity);
                enemyInstance.transform.localScale = new Vector3(scale, scale, scale);
                instantiatedEnemies.Add(enemyInstance);
            }
        }
    }
}
