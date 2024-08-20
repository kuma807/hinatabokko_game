using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Numerics;
using Unity.VisualScripting;

public class GameRenderer : MonoBehaviour
{
    private List<GameObject> instantiatedEnemies = new List<GameObject>();
    private List<GameObject> instantiatedCells = new List<GameObject>();
    private List<GameObject> instantiatedCards = new List<GameObject>();
    private List<GameObject> instantiatedWaveClearPopupObjects = new List<GameObject>();
    private List<GameObject> instantiatedWaveFailPopupObjects = new List<GameObject>();
    private List<GameObject> instantiatedStageClearPopupObjects = new List<GameObject>();
    public static GameRenderer Instance { get; private set; }
    public List<GameObject> enemyObjects;
    public GameObject cellObject;
    public GameObject cardObject;
    public List<GameObject> cardObjects;
    public GameObject canvas;
    public GameObject WaveClearPopup;
    public GameObject WaveFailPopup;
    public GameObject StageClearPopup;
    public TextMeshProUGUI GoalCount;

    
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

    public void CreateCell(ref Board board)
    {
        for (int i = 0; i < board.Count; i++)
        {
            GameObject instantiatedCell = Instantiate(cellObject, new UnityEngine.Vector3(board[i].x, board[i].y, 0), UnityEngine.Quaternion.identity);
            instantiatedCells.Add(instantiatedCell);
        }
    }

    public int GetCellIndex(GameObject cell)
    {
        for (int i = 0; i < instantiatedCells.Count; i++)
        {
            if (instantiatedCells[i].transform.position == cell.transform.position)
            {
                return i;
            }
        }
        return -1;
    }

    public void CreateCards(ref Inventory inventory)
    {
        foreach (Card card in inventory.cards)
        {
            GameObject instantiatedCard = Instantiate(cardObjects[card.effect.id], new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
            instantiatedCard.transform.SetParent(canvas.transform.Find("Inventory"), false);
            instantiatedCards.Add(instantiatedCard);
            // instantiatedCardの子コンポーネントのtextを取得して，textの値を
        }
    }

    public int GetCardIndex(Card card) 
    {
        for (int i = 0; i < instantiatedCards.Count; i++)
        {
            if (instantiatedCards[i].transform.position == card.transform.position)
            {
                return i;
            }
        }
        return -1;
    }

    public void DeleteEnemy()
    {
        foreach (GameObject enemyInstance in instantiatedEnemies)
        {
            if (enemyInstance != null)
            {
                Destroy(enemyInstance);
            }
        }
        instantiatedEnemies.Clear();
        for (int i = 0; i < instantiatedCells.Count; i++)
        {
            TextMeshProUGUI[] textComponents = instantiatedCells[i].GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI tmp in textComponents)
            {
                if (tmp.gameObject.name == "NumberText")
                {
                    tmp.text = "0";
                    break;
                }
            }
        }
    }

    public void CreateEnemy(ref Board board)
    {
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
        for (int i = 0; i < instantiatedCells.Count; i++)
        {
            TextMeshProUGUI[] textComponents = instantiatedCells[i].GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (TextMeshProUGUI tmp in textComponents)
            {
                if (tmp.gameObject.name == "NumberText")
                {
                    tmp.text = board[i].enemy.count.ToString();
                    break;
                }
            }
        }
    }

    public void UpdateEnemy(ref Board board) 
    {
        DeleteEnemy();
        CreateEnemy(ref board);
    }

    public void CreateWaveClearPopup(Enemy enemy)
    {
        GameObject backGround = Instantiate(WaveClearPopup, new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
        backGround.transform.SetParent(canvas.transform, false);
        TextMeshProUGUI[] textComponents = backGround.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (TextMeshProUGUI tmp in textComponents)
        {
            if (tmp.gameObject.name == "DiceFaces")
            {
                tmp.text = string.Join(", ", enemy.dice);
                break;
            }
        }
        GameObject enemyInstance = Instantiate(enemyObjects[enemy.id], new UnityEngine.Vector3((float)-3.64, (float)0.32,0), UnityEngine.Quaternion.identity);
        enemyInstance.transform.localScale = new UnityEngine.Vector3(3, 3, 3);
        enemyInstance.GetComponent<Renderer>().sortingLayerName = "UI";
        backGround.transform.SetParent(canvas.transform);
        instantiatedWaveClearPopupObjects.Add(backGround);
        instantiatedWaveClearPopupObjects.Add(enemyInstance);
    }

    public void DeleteWaveClearPopup()
    {
        for (int i = 0; i < instantiatedWaveClearPopupObjects.Count; i++)
        {
            Destroy(instantiatedWaveClearPopupObjects[i]);
        }
        instantiatedWaveClearPopupObjects.Clear();
    }

    public void CreateWaveFailPopup()
    {
        GameObject instantiatedWaveFailPopup = Instantiate(WaveFailPopup, new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
        instantiatedWaveFailPopup.transform.SetParent(canvas.transform, false);
        instantiatedWaveFailPopupObjects.Add(instantiatedWaveFailPopup);
    }

    public void DeleteWaveFailPopup()
    {
        for (int i = 0; i < instantiatedWaveFailPopupObjects.Count; i++)
        {
            Destroy(instantiatedWaveFailPopupObjects[i]);
        }
        instantiatedWaveFailPopupObjects.Clear();
    }

    public void CreateStageClearPopup()
    {
        GameObject stageClearPopupInstance = Instantiate(StageClearPopup, new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
        stageClearPopupInstance.transform.SetParent(canvas.transform, false);
        instantiatedStageClearPopupObjects.Add(stageClearPopupInstance);
    }

    public void DeleteStageClearPopup()
    {
        for (int i = 0; i < instantiatedStageClearPopupObjects.Count; i++)
        {
            Destroy(instantiatedStageClearPopupObjects[i]);
        }
        instantiatedStageClearPopupObjects.Clear();
    }

    public void DisplayGoalCount(BigInteger x)
    {
        GoalCount.text = x.ToString();
    }
}
