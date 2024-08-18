using System;
using System.Collections.Generic;
public abstract class Effect
{
    public abstract List<double> effect(ref Board board, ref Cell cell);
    public int id;
}

public class NoEffect : Effect
{
    public NoEffect()
    {
        this.id = 0;
    }

    public override List<double> effect(ref Board board, ref Cell cell)
    {
        int size = board.Count;
        List<double> res = new List<double>(size);
        res[cell.index] = 1;
        return res;
    }
}

public class BackEffect : Effect
{
    int back;
    public BackEffect()
    {
        this.id = 1;
    }

    public override List<double> effect(ref Board board, ref Cell cell)
    {
        int size = board.Count;
        List<double> res = new List<double>(size);
        res[cell.index] = 1;

        for (int i = 0; i < back; i++)
        {
            List<double> nres = new List<double>(size);
            foreach (Cell c in board)
            {
                if (c.prev_index.Count == 0) continue;
                if (res[c.index] == 0) continue;
                foreach (int b in c.prev_index)
                {
                    nres[b] += res[c.index] / c.prev_index.Count;
                }
            }
            (res, nres) = (nres, res);
        }

        return res;
    }
}

public class StopEffect : Effect
{
    double stop;
    public StopEffect()
    {
        this.id = 2;
    }
    // enemy stops w.p. stop / (stop + 1)

    public override List<double> effect(ref Board board, ref Cell cell)
    {
        int size = board.Count;
        List<double> res = new List<double>(size);

        res[cell.index] = stop / (stop + 1);
        foreach (int next in cell.next_index)
        {
            res[next] += 1 / (cell.next_index.Count) * (1 / (stop + 1));
        }

        return res;
    }
}
