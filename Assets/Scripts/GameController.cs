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
    private bool wavesStart = false;
    private int wave_num = 0;
    private Board board;
    private int turn = -2;
    public string stageName;
    private Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        stage = new Stage(stageName);
        board = stage.waves[wave_num];
        inventory = Inventory.TestInventory();
        GameRenderer.Instance.CreateCell(ref board);
        GameRenderer.Instance.CreateCards(ref inventory);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateTurn()
    {
        if (wave_num < stage.waves.Count)
        {
            if (turn == stage.enemies[wave_num].turn)
            {
                turn = -3;
                wave_num += 1;
                if (wave_num < stage.waves.Count)
                {
                    board = stage.waves[wave_num];
                }
            }
            else
            {
                if (turn == -2)
                {
                    GameRenderer.Instance.CreateWaveClearPopup(stage.enemies[wave_num]);
                    GameRenderer.Instance.DeleteEnemy();
                }
                if (turn == 0)
                {
                    GameRenderer.Instance.DeleteWaveClearPopup();
                    MoveEnemies(ref board);
                    GameRenderer.Instance.UpdateEnemy(ref board);
                }
                else if (turn > 0)
                {
                    MoveEnemies(ref board);
                    GameRenderer.Instance.UpdateEnemy(ref board);
                }
                turn += 1;
            }
        }
        else
        {
            CancelInvoke("UpdateTurn");
            GameRenderer.Instance.DeleteEnemy();
        }
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

    public void SetWavesStart(bool _wavesStart)
    {
        GameRenderer.Instance.DeleteWaveClearPopup();
        wavesStart = _wavesStart;
        InvokeRepeating("UpdateTurn", 0, 1.0f);
    }
}
