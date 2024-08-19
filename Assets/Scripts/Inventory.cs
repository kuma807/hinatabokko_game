using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[System.Diagnostics.DebuggerDisplay("card: {card}")]
public class Inventory
{
    public List<Card> cards;

    public Inventory(List<Card> _cards)
    {
        cards = _cards;
    }

    static public Inventory TestInventory()
    {
        Card back1 = new Card(new BackEffect(1));
        Card back2 = new Card(new BackEffect(2));
        Card stop1 = new Card(new StopEffect(1));
        return new Inventory(new List<Card> { back1, back2,stop1 });
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}
