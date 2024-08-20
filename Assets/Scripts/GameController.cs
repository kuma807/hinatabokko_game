using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Numerics;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

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

    private Stage stage;
    private bool wavesStart = false;
    private int wave_num = 0;
    private Board board;
    private int turn = 0;
    private int multiplier = 1;
    private const int PopupSeconds = 2;
    private int popupSecondsRemaining = PopupSeconds;
    public string stageName;
    private Dictionary<int,List<List<double>>> probMatrices = new Dictionary<int, List<List<double>>>();
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
        // ShowBoard();
    }

    // 1ターン進む（体力はmultiplier分減る）
    void UpdateTurn()
    {
        Debug.Log(popupSecondsRemaining);
        // wave がまだあるとき
        if (wave_num < stage.waves.Count)
        {
            GameRenderer.Instance.DisplayGoalCount(board.enemy_pass_count());
            // 敵が全部倒れたとき
            if (turn >= stage.enemies[wave_num].turn)
            {
                turn = 0;
                wave_num += 1;
                if (wave_num < stage.waves.Count)
                {
                    board = stage.waves[wave_num];
                }
                popupSecondsRemaining = PopupSeconds;
            }
            else
            {
                // ポップアップ表示中
                if (popupSecondsRemaining > 0)
                {
                    if (popupSecondsRemaining == PopupSeconds)
                    {
                        GameRenderer.Instance.CreateWaveClearPopup(stage.enemies[wave_num]);
                        GameRenderer.Instance.DeleteEnemy();
                    }
                    else if (popupSecondsRemaining == 1)
                    {
                        GameRenderer.Instance.DeleteWaveClearPopup();
                        Enemy enemies = stage.enemies[wave_num];
                        GameRenderer.Instance.UpdateEnemy(ref board);
                    }
                    popupSecondsRemaining--;
                }
                else
                {
                    if (turn > 0)
                    {
                        Enemy enemies = stage.enemies[wave_num];
                        MoveEnemiesByProbability(ref board, ref enemies);
                        GameRenderer.Instance.UpdateEnemy(ref board);
                        if (board.enemy_pass_count() > stage.enemyPassLimits[wave_num])
                        {
                            GameRenderer.Instance.CreateWaveFailPopup();
                            CancelInvoke("UpdateTurn");
                        }
                    }
                    turn += multiplier;
                }
            }
        }
        else
        {
            // TODO: 
            // GameRenderer.Instance.CreateStageClearPopup();
            CancelInvoke("UpdateTurn");
            GameRenderer.Instance.DeleteEnemy();
        }
    }

    // 1回の更新で multiplier 分のターンが進む
    void MoveEnemiesByProbability(ref Board board,ref Enemy enemy)
    {
        List<BigInteger> enemiesCount = new List<BigInteger>(board.Count);
        for (int i = 0; i < board.Count; i++)
        {
            enemiesCount.Add(board[i].enemy.count);
        }
        List<BigInteger> nextEnemiesCount = GameCalculater.NTurnsLater(probMatrices[enemy.id],enemiesCount, multiplier);
        for (int i = 0; i < board.Count; i++)
        {
            board[i].enemy.count = nextEnemiesCount[i];
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
        foreach(var enemy in stage.enemies)
        {
            var e = enemy;
            probMatrices.Add(enemy.id, GameCalculater.calc_probability(ref board, ref e));
        }
        GameRenderer.Instance.DeleteWaveClearPopup();
        wavesStart = _wavesStart;
        InvokeRepeating("UpdateTurn", 0, 1.0f);
    }

    public void UseCardOnCell(Card card, GameObject cell)
    {
        int cardIndex = GameRenderer.Instance.GetCardIndex(card);
        int cellIndex = GameRenderer.Instance.GetCellIndex(cell);
        board[cellIndex].set_effect(inventory.cards[cardIndex].effect);
    }

    public void ShowBoard()
    {
        string s = "";
        for (int i = 0; i < board.Count; i++)
        {
            s += (board[i].roll_dice_effect.id + board[i].step_on_effect.id).ToString();
            s += ", ";
        }
        Debug.Log(s);
    }
}
