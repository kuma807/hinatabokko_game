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
    private List<GameObject> instantiatedLines = new List<GameObject>();
    private List<GameObject> instantiatedCells = new List<GameObject>();
    private List<GameObject> instantiatedCards = new List<GameObject>();
    private List<GameObject> instantiatedWaveStartPopupObjects = new List<GameObject>();
    private List<GameObject> instantiatedWaveClearPopupObjects = new List<GameObject>();
    private List<GameObject> instantiatedWaveFailPopupObjects = new List<GameObject>();
    private List<GameObject> instantiatedStageClearPopupObjects = new List<GameObject>();
    private List<GameObject> instantiatedLineObjects = new List<GameObject>();
    private GameObject instantiatedBackGround;
    public static GameRenderer Instance { get; private set; }
    public List<GameObject> enemyObjects;
    public GameObject lineObject;
    public GameObject cellObject;
    public Sprite GoalCellSprite;
    public Sprite StartCellSprite;
    public GameObject cardObject;
    public List<GameObject> cardObjects;
    public GameObject canvas;
    public GameObject WaveStartPopup;
    public GameObject WaveClearPopup;
    public GameObject WaveFailPopup;
    public GameObject StageClearPopup;
    public GameObject GameClearPopup;
    public List<GameObject> BackGrounds;
    public List<Sprite> CellIconSprites;
    public List<double> CellIconScale;
    public TextMeshProUGUI GoalCount;
    public TextMeshProUGUI MaxGoalCount;
    public TextMeshProUGUI GoalPercent;
    public TextMeshProUGUI TurnLeft;
    public TextMeshProUGUI TutorialText;
    public TextMeshProUGUI MultiplierText;
    
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
        CellIconScale = new List<double>{1.0, 2, 0.15, 0.125, 0.125, 0.15};
    }

    public void InitStage(ref Board board, ref Inventory inventory, int backGroundNumber)
    {
        DeleteGameObjects(ref instantiatedEnemies);
        DeleteGameObjects(ref instantiatedCells);
        DeleteGameObjects(ref instantiatedCards);
        DeleteGameObjects(ref instantiatedWaveClearPopupObjects);
        DeleteGameObjects(ref instantiatedWaveFailPopupObjects);
        DeleteGameObjects(ref instantiatedStageClearPopupObjects);
        DeleteGameObjects(ref instantiatedLineObjects);
        CreateCell(ref board);
        CreateCards(ref inventory);
        CreateGameBackGround(backGroundNumber);
        GoalCount.text = "0";
        MaxGoalCount.text = "0";
        GoalPercent.text = "0%";
        GoalPercent.color = Color.black;
    }

    public void CreateCell(ref Board board)
    {
        for (int i = 0; i < board.Count; i++)
        {
            bool isGoal = false;
            bool isStart = false;
            for (int j = 0; j < board.goal.Count; j++)
            {
                if (i == board.goal[j])
                {
                    isGoal = true;
                }
            }
            for (int j = 0; j < board.start.Count; j++)
            {
                if (i == board.start[j])
                {
                    isStart = true;
                }
            }
            GameObject instantiatedCell = Instantiate(cellObject, new UnityEngine.Vector3(board[i].x, board[i].y, 0), UnityEngine.Quaternion.identity);
            if (isGoal)
            {
                Transform iconObject = instantiatedCell.transform.Find("Icon");
                iconObject.localScale = new UnityEngine.Vector3(0.15f, 0.15f, 0.15f);
                iconObject.GetComponent<SpriteRenderer>().sprite = GoalCellSprite;
            }
            if (isStart)
            {
                Transform iconObject = instantiatedCell.transform.Find("Icon");
                iconObject.localScale = new UnityEngine.Vector3(0.3f, 0.3f, 0.3f);
                iconObject.GetComponent<SpriteRenderer>().sprite = StartCellSprite;
            }
            instantiatedCells.Add(instantiatedCell);
            
            foreach (int toCell_idx in board[i].next_index)
            {
                GameObject instantiatedLine = Instantiate(lineObject, new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
                LineRenderer line = instantiatedLine.GetComponent<LineRenderer>();
                // 頂点の数
                line.positionCount = 2;
                line.SetPosition(0, new UnityEngine.Vector3(board[i].x, board[i].y, 0));
                line.SetPosition(1, new UnityEngine.Vector3(board[toCell_idx].x, board[toCell_idx].y, 0));
                instantiatedLineObjects.Add(instantiatedLine);
            }
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
        foreach (Effect cardEffect in inventory.cardEffects)
        {
            GameObject instantiatedCard = Instantiate(cardObjects[cardEffect.id], new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
            instantiatedCard.transform.SetParent(canvas.transform.Find("Inventory"), false);
            instantiatedCards.Add(instantiatedCard);
            // instantiatedCardの子コンポーネントのtextを取得して，textの値を
            if (cardEffect is BackEffect tmpEffect)
            {
                Text[] textComponents = instantiatedCard.GetComponentsInChildren<Text>(true);
                foreach (Text tmp in textComponents)
                {
                    if (tmp.gameObject.name == "Text (Legacy)")
                    {
                        tmp.text = "Back " + tmpEffect.back.ToString();
                        break;
                    }
                }
            }
            if (cardEffect is StopEffect tmpEffect2)
            {
                Text[] textComponents = instantiatedCard.GetComponentsInChildren<Text>(true);
                foreach (Text tmp in textComponents)
                {
                    if (tmp.gameObject.name == "Text (Legacy)")
                    {
                        tmp.text = "Stop " + tmpEffect2.stop.ToString();
                        break;
                    }
                }
            }
            if (cardEffect is DeathEffect tmpeffect3)
            {
                Text[] textComponents = instantiatedCard.GetComponentsInChildren<Text>(true);
                foreach (Text tmp in textComponents)
                {
                    if (tmp.gameObject.name == "Text (Legacy)")
                    {
                        tmp.text = "Death " + (tmpeffect3.death_probability * 100).ToString() + "%";
                        break;
                    }
                }
            }
        }
    }

    public int GetCardIndex(Card card) 
    {
        for (int i = 0; i < instantiatedCards.Count; i++)
        {
            if (card.transform.parent.gameObject == instantiatedCards[i])
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

    public void CreateWaveStartPopup(Enemy enemy)
    {
        GameObject backGround = Instantiate(WaveStartPopup, new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
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
        instantiatedWaveStartPopupObjects.Add(backGround);
        instantiatedWaveStartPopupObjects.Add(enemyInstance);
    }

    public void DeleteWaveStartPopup()
    {
        DeleteGameObjects(ref instantiatedWaveStartPopupObjects);
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

    public void CreateGameClearPopup()
    {
        GameObject gameClearPopupInstance = Instantiate(GameClearPopup, new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
        gameClearPopupInstance.transform.SetParent(canvas.transform, false);
    }

    public void CreateGameBackGround(int backGroundNumber)
    {
        if (instantiatedBackGround != null)
        {
            Destroy(instantiatedBackGround);
        }
        instantiatedBackGround = Instantiate(BackGrounds[backGroundNumber], new UnityEngine.Vector3(0, 0, 0), UnityEngine.Quaternion.identity);
    }

    public void DisplayGoalCount(BigInteger x)
    {
        GoalCount.text = x.ToString();
    }

    public void DisplayMaxGoalCount(BigInteger x)
    {
        MaxGoalCount.text = x.ToString();
    }

    public void DisplayGoalPercent(BigInteger x)
    {
        GoalPercent.text = x.ToString() + "%";
        if (x < 0)
        {
            GoalPercent.color = Color.red;
        }
        if (x >= 0)
        {
            GoalPercent.color = Color.black;
        }
    }

    public void DisplayTurnLeft(BigInteger x)
    {
        TurnLeft.text = x.ToString();
    }

    public void DisplayTutorial(string s)
    {
        TutorialText.text = s;
    }

    public void DisplayMultiplier(BigInteger x)
    {
        MultiplierText.text = x.ToString();
    }

    public void ChangeCellIcon(int cellIndex, int effectId)
    {
        GameObject instantiatedCell = instantiatedCells[cellIndex];
        SpriteRenderer spriteRenderer = instantiatedCell.transform.Find("Icon").GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = CellIconSprites[effectId];
        spriteRenderer.transform.localScale = new UnityEngine.Vector3((float)CellIconScale[effectId], (float)CellIconScale[effectId], (float)CellIconScale[effectId]);
    }

    public void DeleteGameObjects(ref List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            Destroy(gameObjects[i]);
        }
        gameObjects.Clear();
    }
}
