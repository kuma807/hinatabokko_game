using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameRenderer : MonoBehaviour
{
    private List<GameObject> instantiatedEnemies = new List<GameObject>();
    public static GameRenderer Instance { get; private set; }
    public List<GameObject> enemyObjects;
    public GameObject cellObject;
    
    private void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン変更時にオブジェクトを保持
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateCell(ref Board board)
    {
        for (int i = 0; i < board.Count; i++)
        {
            Instantiate(cellObject, new UnityEngine.Vector3(board[i].x, board[i].y, 0), UnityEngine.Quaternion.identity);
        }
    }

    public void UpdateEnemy(ref Board board) 
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
