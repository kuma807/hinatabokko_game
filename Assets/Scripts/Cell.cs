using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cell
{
    public int x;
    public int y;
    public Enemy enemy;
    public int index;
    public List<int> next_index;
    public List<int> prev_index;
    public StepOnEffect step_on_effect = new NullStepOnEffect();
    public RollDiceEffect roll_dice_effect = new NullRollDiceEffect();

    public Cell(int _x, int _y, Enemy _enemy, int _index)
    {
        x = _x;
        y = _y;
        enemy = _enemy;
        index = _index;
        // calculate next_index, prev_index
    }
    public Cell(int _x, int _y, Enemy _enemy, int _index, StepOnEffect _s)
    {
        x = _x;
        y = _y;
        enemy = _enemy;
        index = _index;
        step_on_effect = _s;
        // calculate next_index, prev_index
    }

    public Cell(int _x, int _y, Enemy _enemy, int _index, RollDiceEffect _r)
    {
        x = _x;
        y = _y;
        enemy = _enemy;
        index = _index;
        roll_dice_effect = _r;
        // calculate next_index, prev_index
    }

    public void set_step_on_effect(StepOnEffect _s)
    {
        step_on_effect = _s;
    }

    public void set_roll_dice_effect(RollDiceEffect _r)
    {
        roll_dice_effect = _r;
    }

    public void reset_step_on_effect()
    {
        step_on_effect = new NullStepOnEffect();
    }
    public void reset_roll_dice_effect()
    {
        roll_dice_effect = new NullRollDiceEffect();
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log("Cell coordination: (" + x + ", " + y + ")");
        enemy.DisplayInfo();
    }
}
