using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameCalculater
{
    //1ターン後の遷移確率
    public static List<List<double>> calcProbability(ref Board board, ref Enemy enemy)
    {
        var dice = enemy.dice;

        return null;
    }

    //Tターン後の遷移確率を行列累乗で計算
    public static List<List<double>> updateTurn(List<List<double>> prob, long turn)
    {
        return null;
    }
}
