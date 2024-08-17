using System;

namespace GMTKGameJam.Scripts;
public abstract class Effect
{
    public abstract void effect(ref Board board, ref Cell cell)
}

public class BackEffect : Effect
{
    int back;

    public override double[] effect(ref Board board, ref Cell cell)
    {
        const int size = board.size();
        double[] res = new double[size];
        res[cell.index] = 1;

        for (int i = 0; i < back; i++)
        {
            double[] nres = new double[size];
            foreach (Cell c in board.cells)
            {
                if (c.prev_index.size() == 0) continue;
                if (res[c.index] == 0) continue;
                foreach (int b in c.prev_index)
                {
                    nres[b] += res[c.index] / c.prev_index.size();
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

    public override void effect(ref Board board, ref Cell cell)
    {
        const int size = board.size();
        double[] res = new double[size];

        res[cell.index] = stop / (stop + 1);
        foreach (int next: cell.next_index)
        {
            res[next] += 1 / (next_index.size()) * (1 / (stop + 1));
        }

        return res;
    }
}
