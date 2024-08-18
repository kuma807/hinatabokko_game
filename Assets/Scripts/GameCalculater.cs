using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Tターン後の遷移確率を行列累乗で計算
    public static List<List<double>> updateTurn(List<List<double>> prob, long turn)
    {
        return null;
    }
}
