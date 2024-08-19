using System;
using System.Collections.Generic;
using System.Numerics;
using System.Buffers;
using UnityEngine.UIElements;
public abstract class Effect
{
    public abstract List<double> effect(Board board, Cell cell, Enemy enemy);
    public int id;
}

public abstract class StepOnEffect : Effect { }
public abstract class RollDiceEffect : Effect { }

public class NullStepOnEffect : StepOnEffect
{
    public NullStepOnEffect()
    {
        this.id = 0;
    }

    public override List<double> effect(Board board, Cell cell, Enemy enemy)
    {
        int size = board.Count;
        List<double> res = new List<double>(size);
        for (int i = 0; i < size; i++) res.Add(0);
        res[cell.index] = 1;
        return res;
    }
}

public class NullRollDiceEffect : RollDiceEffect
{
    public NullRollDiceEffect()
    {
        this.id = 0;
    }

    public override List<double> effect(Board board, Cell cell, Enemy enemy)
    {
        int size = board.Count;
        List<double> res = new List<double>(size);
        for (int i = 0; i < size; i++) res.Add(0);
        foreach (int next in enemy.dice)
        {
            res[Math.Min(size - 1, cell.index + next)] += 1.0 / enemy.dice.Count;
        }
        return res;
    }
}


public class BackEffect : StepOnEffect
{
    int back;
    public BackEffect(int _back)
    {
        this.id = 1;
        back = _back;
    }

    public override List<double> effect(Board board, Cell cell, Enemy enemy)
    {
        int size = board.Count;
        List<double> res = new List<double>(size);
        for (int i = 0; i < size; i++) res.Add(0);
        res[cell.index] = 1;

        for (int i = 0; i < back; i++)
        {
            List<double> nres = new List<double>(size);
            for (int j = 0; j < size; j++)
            {
                nres.Add(0);
            }
            foreach (Cell c in board)
            {
                if (c.prev_index.Count == 0) continue;
                if (res[c.index] == 0) continue;

                foreach (int b in c.prev_index)
                {
                    nres[b] += res[c.index] / c.prev_index.Count;
                }
            }
            (res, _) = (nres, res);
        }

        return res;
    }
}

public class StopEffect : RollDiceEffect
{
    double stop;
    public StopEffect(double _stop)
    {
        this.id = 2;
        stop = _stop;
    }
    // enemy stops w.p. stop / (stop + 1)

    public override List<double> effect(Board board, Cell cell, Enemy enemy)
    {
        int size = board.Count;
        List<double> res = new List<double>(size);
        for (int i = 0; i < size; i++) res.Add(0);

        res[cell.index] = stop / (stop + 1);
        foreach (int next in enemy.dice)
        {
            List<double> step = board.StepN(cell.index, next);
            // �����̂Ƃ� step[index+next] = 1, 0 o/w.

            for (int i = 0; i < size; i++)
            {
                res[i] += step[i] / enemy.dice.Count * (1 / (stop + 1));
            }
            //res[Math.Min(size - 1, cell.index + next)] += 1.0 / enemy.dice.Count * (1 / (stop + 1));
        }

        return res;
    }
}

public class DeathEffect : StepOnEffect
{
    double death_probability;
    // enemy will be dead w.p. death_probability, be alive w.p. 1 - death_probability
    public DeathEffect(double _death_probability)
    {
        death_probability = _death_probability;
        this.id = 3;
    }
    public override List<double> effect(Board board, Cell cell, Enemy enemy)
    {
        var res = new List<double>(board.Count);
        for (int i = 0; i < board.Count; i++) res.Add(0);
        res[cell.index] = Math.Max(0, 1 - death_probability);
        return res;
    }
}

public class BackStartEffect : StepOnEffect
{
    public BackStartEffect() { 
        this.id = 4;
    }
    public override List<double> effect(Board board, Cell cell, Enemy enemy)
    {
        var res = new List<double>(board.Count);
        for (int i = 0; i < board.Count; i++) res.Add(0);
        foreach (int s in board.start)
        {
            res[s] = 1.0 / board.start.Count;
        }
        return res;
    }
}

public class ReverseEffect: RollDiceEffect
{
    public ReverseEffect() {
        this.id = 5;
    }
    public override List<double> effect(Board board, Cell cell, Enemy enemy) 
    {
        var res = new List<double>(board.Count);
        foreach (int next in enemy.dice)
        {
            var nres = board.BackN(cell.index, next);
            for (int i = 0; i < board.Count; i++)
            {
                res[i] += nres[i] * (1.0 / enemy.dice.Count);
            }
        }
        return res;
    }
}
