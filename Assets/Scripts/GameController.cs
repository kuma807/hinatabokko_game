using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

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
            new Cell(2, 0, new Enemy(0, new List<int>{1, 2, 3, 4, 5, 6}, 0), 1, new NullStepOnEffect(), new NullRollDiceEffect()),
            new Cell(4, 0, new Enemy(0, new List<int>{1, 2, 3, 4, 5, 6}, 0), 2, new NullStepOnEffect(), new NullRollDiceEffect()),
            new Cell(6, 0, new Enemy(0, new List<int>{1, 2, 3, 4, 5, 6}, 0), 3, new NullStepOnEffect(), new NullRollDiceEffect()),
            new Cell(8, 0, new Enemy(0, new List<int>{1, 2, 3, 4, 5, 6}, 0), 4, new NullStepOnEffect(), new NullRollDiceEffect()),
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
                GameObject enemyInstance = Instantiate(enemyObjects[board[i].enemy.id], new Vector3(board[i].x, board[i].y + (float)(0.3 - 0.3 * board[i].enemy.id), 0), Quaternion.identity);
                TextMeshProUGUI textMeshPro = enemyInstance.GetComponentInChildren<TextMeshProUGUI>();
                if (textMeshPro != null)
                {
                    textMeshPro.text = board[i].enemy.count.ToString();  // テキストを更新
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
