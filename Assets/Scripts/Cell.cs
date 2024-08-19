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

    public Cell(int _x, int _y, Enemy _enemy, int _index, List<int> _prev_index, List<int> _next_index)
    {
        x = _x;
        y = _y;
        enemy = _enemy;
        index = _index;
        prev_index = _prev_index;
        next_index = _next_index;
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

    public void set_effect(Effect effect)
    {
        if (effect is StepOnEffect stepEffect)
        {
            set_step_on_effect(stepEffect);
            reset_roll_dice_effect();
        }
        if (effect is RollDiceEffect diceEffect)
        {
            reset_step_on_effect();
            set_roll_dice_effect(diceEffect);
        }
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
