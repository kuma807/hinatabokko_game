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
    private int wave_num = 0;
    private Board board;
    private int turn = 0;
    public string stageName;

    // Start is called before the first frame update
    void Start()
    {
        stage = new Stage(stageName);
        board = stage.waves[wave_num];
        GameRenderer.Instance.CreateCell(ref board);
        GameRenderer.Instance.UpdateEnemy(ref board);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (turn == stage.enemies[wave_num].turn)
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
                GameRenderer.Instance.UpdateEnemy(ref board);
            }
            if (wave_num < stage.waves.Count)
            {
                UpdateTurn();
            }
        }
    }

    void UpdateTurn()
    {
        MoveEnemies(ref board);
        GameRenderer.Instance.UpdateEnemy(ref board);
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
}
