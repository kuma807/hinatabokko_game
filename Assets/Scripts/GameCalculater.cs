using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.U2D.IK;

public static class GameCalculater
{
    //1ターン後の遷移確率
    public static List<List<double>> calc_probability(ref Board board, ref Enemy enemy)
    {
        int n = board.Count;
        var res = new List<List<double>>(n);
        for (int i = 0; i < n; i++) res[i] = new List<double>(n);

        foreach (Cell cell in board)
        {
            var v1 = cell.roll_dice_effect.effect(board, cell, enemy);
            for (int i = 0; i < n; i++)
            {
                if (i == cell.index) continue;
                var v2 = board[i].step_on_effect.effect(board, board[i], enemy);
                for (int j = 0; i < n; j++) res[j][cell.index] += v1[i] * v2[j];
            }
        }

        return res;
    }

    public static List<List<double>> product(List<List<double>> A, List<List<double>> B)
    {
        Debug.Assert(A[0].Count == B.Count);
        var C = new List<List<double>>(A.Count);
        for (int i = 0; i < A.Count; i++) C[i] = new List<double>(B[0].Count);
        for (int k = 0; k < A[0].Count; k++)
        {
            for (int i = 0; i < A.Count; i++)
            {
                for (int j = 0; j < B[0].Count; j++)
                {
                    C[i][j] += A[i][k] * B[k][j];
                }
            }
        }
        return C;
    }

    //Tターン後の遷移確率を行列累乗で計算
    public static List<List<double>> updateTurn(List<List<double>> prob, long turn)
    {
        var res = new List<List<double>>(prob.Count);
        for (int i = 0; i < prob.Count; i++)
        {
            res[i] = new List<double>(prob.Count);
            res[i][i] = 1;
        }
        while (turn > 1) {
            if ((turn & 1) == 1)
            {
                res = product(res, prob);
            }
            prob = product(prob, prob);
            turn >>= 1;
        }
        return res;
    }

    public static List<double> Act(ref List<List<double>> A, List<double> x)
    {
        Debug.Assert(A[0].Count == x.Count);
        var tmp_vec = new List<List<double>>(x.Count);
        for (int i = 0; i < x.Count; i++) tmp_vec[i].Add(x[i]);
        tmp_vec = product(A, tmp_vec);
        for (int i = 0; i < x.Count; i++) x[i] = tmp_vec[i][0];
        return x;
    }
}
