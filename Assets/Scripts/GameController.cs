using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Numerics;

public enum GameState
{
    preparing = 0,
    enemyIncoming = 1,
    lost = 2,
    won = 3,
    gameClear = 4
}

public enum TutorialState
{
    beforeLeftClickCard = 0,
    beforeLeftClickCell = 1,
    beforeRightClickCell = 2,
    beforeClickStartWaves = 3,
    beforePushEnter = 4,
    beforePushFastForward = 5,
    beforePushSlowDown = 6,
    tutorialEnd = 7,
}

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
    private int wave_num = 0;
    private Board board;
    private BigInteger turn = 0;
    private BigInteger multiplier = 1;
    private const int PopupSeconds = 2;
    private int popupSecondsRemaining = PopupSeconds;
    private int stageNumber = 0;
    private Dictionary<int,List<List<double>>> probMatrices = new Dictionary<int, List<List<double>>>();
    private Inventory inventory;
    private bool finalTurnUpdated;
    public GameState gameState;
    public List<string> stageNames;
    public TutorialState tutorialState;

    // Start is called before the first frame update
    void Start()
    {
        tutorialState = TutorialState.beforeLeftClickCard;
        GameRenderer.Instance.DisplayTutorial("Place trap cards on the board to defend the white castle against enemy attacks. Left Click the trap card to Select trap.");
        InitStage(new Stage(stageNames[stageNumber]), Inventory.TestInventory());
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialState == TutorialState.tutorialEnd)
        {
            if (gameState == GameState.preparing)
            {
                GameRenderer.Instance.DisplayTutorial("Press play button to start enemy attack.");
            }
            if (gameState == GameState.enemyIncoming) 
            {
                GameRenderer.Instance.DisplayTutorial("Press enter to proceed to the next turn.");
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (tutorialState == TutorialState.beforePushEnter)
            {
                tutorialState = TutorialState.beforePushFastForward;
                GameRenderer.Instance.DisplayTutorial("If there is too many turn press fastforward button to fastforward the battle.");
            }
            if (gameState == GameState.won)
            {
                stageNumber += 1;
                if (stageNumber == stageNames.Count)
                {
                    gameState = GameState.gameClear;
                    GameRenderer.Instance.DeleteStageClearPopup();
                    GameRenderer.Instance.CreateGameClearPopup();
                }
                else
                {
                    InitStage(new Stage(stageNames[stageNumber]), inventory);
                }
            }
            else if (gameState == GameState.lost)
            {
                InitStage(new Stage(stageNames[stageNumber]), inventory);
            }
            else if (gameState == GameState.enemyIncoming)
            {
                UpdateTurn();
            }
        }
    }

    // 1ターン進む（体力はmultiplier分減る）
    void UpdateTurn()
    {
        // wave がまだあるとき
        if (wave_num < stage.waves.Count)
        {
            // 敵が全部倒れたとき
            if (finalTurnUpdated)
            {
                turn = 0;
                wave_num += 1;
                if (wave_num < stage.waves.Count)
                {
                    board = stage.waves[wave_num];
                    finalTurnUpdated = false;
                }
                else 
                {
                    gameState = GameState.won;
                    GameRenderer.Instance.CreateStageClearPopup();
                    // CancelInvoke("UpdateTurn");
                    GameRenderer.Instance.DeleteEnemy();
                }
                popupSecondsRemaining = PopupSeconds;
            }
            else if (turn >= stage.enemies[wave_num].turn)
            {
                Enemy enemies = stage.enemies[wave_num];
                MoveEnemiesByProbability(ref board, ref enemies, enemies.turn + multiplier - turn);
                GameRenderer.Instance.UpdateEnemy(ref board);
                if (board.enemy_pass_count() > stage.enemyPassLimits[wave_num])
                {
                    GameRenderer.Instance.CreateWaveFailPopup();
                    gameState = GameState.lost;
                    // CancelInvoke("UpdateTurn");
                }
                UpdateCounter();
                turn = stage.enemies[wave_num].turn + 1;
                finalTurnUpdated = true;
            }
            else
            {
                // ポップアップ表示中
                if (popupSecondsRemaining > 0)
                {
                    if (popupSecondsRemaining == PopupSeconds)
                    {
                        if (wave_num == 0)
                        {
                            GameRenderer.Instance.CreateWaveStartPopup(stage.enemies[wave_num]);
                        }
                        else
                        {
                            GameRenderer.Instance.CreateWaveClearPopup(stage.enemies[wave_num]);
                        }
                        GameRenderer.Instance.DeleteEnemy();
                        UpdateCounter();
                    }
                    else if (popupSecondsRemaining == 1)
                    {
                        if (wave_num == 0)
                        {
                            GameRenderer.Instance.DeleteWaveStartPopup();
                        }
                        else
                        {
                            GameRenderer.Instance.DeleteWaveClearPopup();
                        }
                        Enemy enemies = stage.enemies[wave_num];
                        GameRenderer.Instance.UpdateEnemy(ref board);
                        UpdateCounter();
                        turn += multiplier;
                    }
                    popupSecondsRemaining--;
                }
                else
                {
                    if (turn >= 0)
                    {
                        Enemy enemies = stage.enemies[wave_num];
                        MoveEnemiesByProbability(ref board, ref enemies, multiplier);
                        GameRenderer.Instance.UpdateEnemy(ref board);
                        if (board.enemy_pass_count() > stage.enemyPassLimits[wave_num])
                        {
                            GameRenderer.Instance.CreateWaveFailPopup();
                            gameState = GameState.lost;
                            // CancelInvoke("UpdateTurn");
                        }
                        UpdateCounter();
                    }
                    turn += multiplier;
                }
            }
        }
    }

    // 1回の更新で multiplier 分のターンが進む
    void MoveEnemiesByProbability(ref Board board,ref Enemy enemy, BigInteger mult)
    {
        List<BigInteger> enemiesCount = new List<BigInteger>(board.Count);
        for (int i = 0; i < board.Count; i++)
        {
            enemiesCount.Add(board[i].enemy.count);
        }
        List<BigInteger> nextEnemiesCount = GameCalculater.NTurnsLater(probMatrices[enemy.id],enemiesCount, mult);
        for (int i = 0; i < board.Count; i++)
        {
            board[i].enemy.count = nextEnemiesCount[i];
        }
    }

    public void WavesStart()
    {
        if (gameState != GameState.preparing)
        {
            return;
        }
        if (GameController.Instance.tutorialState == TutorialState.beforeClickStartWaves)
        {
            GameController.Instance.tutorialState = TutorialState.beforePushEnter;
            GameRenderer.Instance.DisplayTutorial("Press Enter to proceed to the next turn.");
        }
        gameState = GameState.enemyIncoming;
        foreach(var enemy in stage.enemies)
        {
            var e = enemy;
            probMatrices.Add(enemy.id, GameCalculater.calc_probability(ref board, ref e));
        }
        GameRenderer.Instance.DeleteWaveClearPopup();
        UpdateCounter();
        UpdateTurn();
        // InvokeRepeating("UpdateTurn", 0, 1.0f);
    }

    public bool UseCardOnCell(Card card, GameObject cell)
    {
        int cardIndex = GameRenderer.Instance.GetCardIndex(card);
        int cellIndex = GameRenderer.Instance.GetCellIndex(cell);
        bool cellUnchangeable = false;
        for (int i = 0; i < board.unchangeable.Count; i++)
        {
            if (board.unchangeable[i] == cellIndex)
            {
                cellUnchangeable = true;
            }
        }
        if (!cellUnchangeable)
        {
            board[cellIndex].set_effect(inventory.cardEffects[cardIndex]);
            GameRenderer.Instance.ChangeCellIcon(cellIndex, inventory.cardEffects[cardIndex].id);
            return true;
        }
        return false;
    }

    public void UpdateCounter()
    {
        BigInteger goalCount = board.enemy_pass_count();
        BigInteger maxGoalCount = stage.enemyPassLimits[wave_num];
        GameRenderer.Instance.DisplayGoalCount(goalCount);
        GameRenderer.Instance.DisplayMaxGoalCount(maxGoalCount);
        GameRenderer.Instance.DisplayGoalPercent((BigInteger)(100 - goalCount * 100 / maxGoalCount));
        BigInteger leftTurn = 0;
        if (leftTurn < stage.enemies[wave_num].turn - turn)
        {
            leftTurn = stage.enemies[wave_num].turn - turn;
        }
        GameRenderer.Instance.DisplayTurnLeft(leftTurn);
    }

    public void FastForward()
    {
        multiplier *= 10;
        if (multiplier > GameCalculater.TEN(30))
        {
            multiplier = GameCalculater.TEN(30);
        }
        GameRenderer.Instance.DisplayMultiplier(multiplier);
        if (tutorialState == TutorialState.beforePushFastForward)
        {
            tutorialState = TutorialState.beforePushSlowDown;
            GameRenderer.Instance.DisplayTutorial("To slow down the battle press slow down button.");
        }
    }

    public void SlowDown()
    {
        if (multiplier != 1)
        {
            multiplier /= 10;
        }
        GameRenderer.Instance.DisplayMultiplier(multiplier);
        if (tutorialState == TutorialState.beforePushSlowDown)
        {
            tutorialState = TutorialState.tutorialEnd;
        }
    }

    public void InitStage(Stage _stage, Inventory _inventory)
    {
        turn = 0;
        wave_num = 0;
        stage = _stage;
        board = stage.waves[wave_num];
        inventory = _stage.inventory;
        gameState = GameState.preparing;
        GameRenderer.Instance.InitStage(ref board, ref inventory, stage.backGroundNumber);
        probMatrices = new Dictionary<int, List<List<double>>>();
        popupSecondsRemaining = PopupSeconds;
        finalTurnUpdated = false;
        if (tutorialState == TutorialState.tutorialEnd)
        {
            GameRenderer.Instance.DisplayTutorial("Press play button to start enemy attack.");
        }
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
