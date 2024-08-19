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
        Card back1 = new Card(new BackStartEffect());
        Card back2 = new Card(new StopEffect(2));
        Card stop1 = new Card(new DeathEffect((float)0.5));
        Card backStart = new Card(new BackStartEffect());
        Card reverseEffect = new Card(new ReverseEffect());
        return new Inventory(new List<Card> { back1, back2, stop1, backStart, reverseEffect});
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}
