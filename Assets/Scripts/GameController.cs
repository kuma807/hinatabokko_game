using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public GameObject enemyObject;
    private Stage stage;
    private List<GameObject> instantiatedEnemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Board board = new Board()
        {
            new Cell(0, 0, 1, 0),
            new Cell(2, 0, 0, 1),
            new Cell(4, 0, 0, 2),
            new Cell(6, 0, 0, 3),
            new Cell(8, 0, 0, 4)
        };
        List<Enemy> enemies = new List<Enemy>() 
        {
            new Enemy(1, new List<int>{1, 2, 3, 4, 5, 6})
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
        board[board.Count - 1].enemyNumber += board[board.Count - 2].enemyNumber;
        for (int i = board.Count - 3; i >= 0; i--)
        {
            board[i + 1].enemyNumber = board[i].enemyNumber;
        }
        board[0].enemyNumber = 0;
        string outputString = "Board";
        for (int i = 0; i < board.Count; i++)
        {
            outputString += " " + board[i].enemyNumber + ",";
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
            if (board[i].enemyNumber != 0)
            {
                GameObject enemyInstance = Instantiate(enemyObject, new Vector3(board[i].x, board[i].y, 0), Quaternion.identity);
                instantiatedEnemies.Add(enemyInstance);
            }
        }
    }
}
