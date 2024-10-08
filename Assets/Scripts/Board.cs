using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;

[System.Diagnostics.DebuggerDisplay("cells: {cells}")]
public class Board: List<Cell>
{
    public List<int> start, goal, unchangeable;
    public Board() {
        start = new List<int>{};
        goal = new List<int>{};
        unchangeable = new List<int>{};
    }
    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }

    public void SetGoal(List<int> _goal)
    {
        goal = _goal;
    }

    public void SetStart(List<int> _start)
    {
        start = _start;
    }

    public void SetUnchangealbe(List<int> _unchangeable)
    {
        unchangeable = _unchangeable;
    }
    
    public List<double> StepN(int index,int n)
    {
        List<double> result = new List<double>(Count);
        for (int i = 0; i < Count; i++)
        {
            result.Add(0);
        }
        if (n == 0)
        {
            result[index] = 1;
        }
        else
        {
            List<double> prevResult = StepN(index, n - 1);

            for (int i = 0; i < Count; i++)
            {
                List<int> next = this[i].next_index;
                foreach(int ne in next)
                {
                    result[ne] += prevResult[i] / next.Count;
                }
            }
        }
        return result;
    }
    public List<double> BackN(int index, int n)
    {
        List<double> result = new List<double>(Count);
        for (int i = 0; i < Count; i++)
        {
            result.Add(0);
        }
        if (n == 0)
        {
            result[index] = 1;
        }
        else
        {
            List<double> prevResult = BackN(index, n - 1);

            for (int i = 0; i < Count; i++)
            {
                List<int> prev = this[i].prev_index;
                foreach (int pre in prev)
                {
                    result[pre] += prevResult[i] / prev.Count;
                }
            }
        }
        return result;
    }

    public BigInteger enemy_pass_count()
    {
        BigInteger res = 0;
        foreach (int i in goal)
        {
            res += this[i].enemy.count;
        }
        return res;
    }
}
