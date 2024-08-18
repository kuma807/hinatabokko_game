using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject[] enemyObjects;
    private Stage stage;
    private List<GameObject> instantiatedEnemies = new List<GameObject>();

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
            new Cell(0, 0, enemies, 0, new NoEffect()),
            new Cell(2, 0, new List<Enemy>(), 1, new NoEffect()),
            new Cell(4, 0, new List<Enemy>(), 2, new NoEffect()),
            new Cell(6, 0, new List<Enemy>(), 3, new NoEffect()),
            new Cell(8, 0, new List<Enemy>(), 4, new NoEffect())
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
        for (int i = 0; i < board[board.Count - 2].enemies.Count; i++) 
        {
            bool foundSameType = false;
            for (int j = 0; j < board[board.Count - 1].enemies.Count; j++)
            {
                if (board[board.Count - 2].enemies[i].dice == board[board.Count - 1].enemies[j].dice)
                {
                    foundSameType = true;
                    board[board.Count - 1].enemies[j].count += board[board.Count - 2].enemies[i].count;
                }
            }
            if (!foundSameType)
            {
                board[board.Count - 1].enemies.Add(board[board.Count - 2].enemies[i]);
            }
        }
        for (int i = board.Count - 3; i >= 0; i--)
        {
            board[i + 1].enemies = board[i].enemies;
        }
        board[0].enemies = new List<Enemy>();
        string outputString = "Board";
        for (int i = 0; i < board.Count; i++)
        {
            for (int j = 0; j < board[i].enemies.Count; j++)
            outputString += " " + board[i].enemies[j].count + ",";
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
            for (int j = 0; j < board[i].enemies.Count; j++)
            {
                if (board[i].enemies[j].count != 0)
                {
                    GameObject enemyInstance = Instantiate(enemyObjects[board[i].enemies[j].id], new Vector3(board[i].x, board[i].y + (float)(0.3 - 0.3 * board[i].enemies[j].id), 0), Quaternion.identity);
                    TextMeshProUGUI textMeshPro = enemyInstance.GetComponentInChildren<TextMeshProUGUI>();
                    if (textMeshPro != null)
                    {
                        textMeshPro.text = board[i].enemies[j].count.ToString();  // テキストを更新
                    }
                    else
                    {
                        Debug.LogWarning("TextMeshProUGUI component not found in the prefab's children.");
                    }
                    instantiatedEnemies.Add(enemyInstance);
                }
            }
        }
    }
}
