using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Cell[] board;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        board = new Cell[]
        {
            new Cell(0, 0, 1),
            new Cell(2, 0, 0),
            new Cell(4, 0, 0),
            new Cell(6, 0, 0),
            new Cell(8, 0, 0)
        };
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
        board[board.Length - 1].enemyNumber += board[board.Length - 2].enemyNumber;
        for (int i = board.Length - 3; i >= 0; i--)
        {
            board[i + 1].enemyNumber = board[i].enemyNumber;
        }
        board[0].enemyNumber = 0;
        string outputString = "Board";
        for (int i = 0; i < board.Length; i++)
        {
            outputString += " " + board[i].enemyNumber + ",";
        }
        Debug.Log(outputString);
    }

    void DrawEnemy()
    {
        for (int i = 0; i < board.Length; i++)
        {
            
        }
    }
}
