﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
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
        for (int i = 0; i < n; i++)
        {
            res.Add(new List<double>(n));
            for (int j = 0; j < n; j++)
            {
                res[i].Add(0);
            }
        }

        foreach (Cell cell in board)
        {
            var v1 = cell.roll_dice_effect.effect(board, cell, enemy);
            for (int i = 0; i < n; i++)
            {
                var v2 = board[i].step_on_effect.effect(board, board[i], enemy);
                if (i == cell.index)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i == j) v2[j] = 1;
                        else v2[j] = 0;
                    }
                }
                for (int j = 0; j < n; j++) res[j][cell.index] += v1[i] * v2[j];
            }
        }

        return res;
    }

    public static List<List<double>> product(List<List<double>> A, List<List<double>> B)
    {
        Debug.Assert(A[0].Count == B.Count);
        var C = new List<List<double>>(A.Count);
        for (int i = 0; i < A.Count; i++)
        {
            C.Add(new List<double>(B[0].Count));
            for (int j = 0; j < B[0].Count; j++)
            {
                C[i].Add(0);
            }
        }
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
    public static List<List<double>> updateTurn(List<List<double>> prob, BigInteger turn)
    {
        var res = new List<List<double>>(prob.Count);
        for (int i = 0; i < prob.Count; i++)
        {
            res.Add(new List<double>(prob.Count));
            for (int j = 0; j < prob.Count; j++)
            {
                res[i].Add(0);
            }
        }
        for (int i = 0; i < prob.Count; i++) res[i][i] = 1;
        while (turn > 0)
        {
            if ((turn & 1) == 1)
            {
                res = product(prob, res);
            }
            prob = product(prob, prob);
            turn >>= 1;
        }
        return res;
    }

    public static List<BigInteger> Act(ref List<List<double>> A, List<BigInteger> x)
    {
        Debug.Assert(A[0].Count == x.Count);
        var tmp_vec = new List<List<double>>(x.Count);
        for (int i = 0; i < x.Count; i++)
        {
            tmp_vec.Add(new List<double>(1));
            tmp_vec[i].Add((double)x[i]);
        }
        BigInteger s = 0, s2 = 0;
        tmp_vec = product(A, tmp_vec);
        for (int i = 0; i < x.Count; i++) {
            s += x[i];
            x[i] = (BigInteger)tmp_vec[i][0];
            s2 += x[i];
        }
        return x;
    }

    public static List<BigInteger> NTurnsLater(in List<List<double>> probMat,List<BigInteger> x,BigInteger turn)
    {
        var powMat = updateTurn(probMat, turn);
        return Act(ref powMat, x);
    }

    public static BigInteger TEN(int n)
    {
        if (n == 0) return 1;
        else return 10 * TEN(n - 1);
    }
}
