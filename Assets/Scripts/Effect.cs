using System;
public abstract class Effect
{
    public abstract double[] effect(ref Board board, ref Cell cell);
}

public class BackEffect : Effect
{
    int back;

    public override double[] effect(ref Board board, ref Cell cell)
    {
        int size = board.Count;
        double[] res = new double[size];
        res[cell.index] = 1;

        for (int i = 0; i < back; i++)
        {
            double[] nres = new double[size];
            foreach (Cell c in board)
            {
                if (c.prev_index.Length == 0) continue;
                if (res[c.index] == 0) continue;
                foreach (int b in c.prev_index)
                {
                    nres[b] += res[c.index] / c.prev_index.Length;
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
    // enemy stops w.p. stop / (stop + 1)

    public override double[] effect(ref Board board, ref Cell cell)
    {
        int size = board.Count;
        double[] res = new double[size];

        res[cell.index] = stop / (stop + 1);
        foreach (int next in cell.next_index)
        {
            res[next] += 1 / (cell.next_index.Length) * (1 / (stop + 1));
        }

        return res;
    }
}
