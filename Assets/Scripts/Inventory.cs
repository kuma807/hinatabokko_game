using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[System.Diagnostics.DebuggerDisplay("card: {card}")]
public class Inventory
{
    public List<Effect> cardEffects;

    public Inventory(List<Effect> _cardEffects)
    {
        cardEffects = _cardEffects;
    }

    static public Inventory TestInventory()
    {
        Effect back1 = new BackStartEffect();
        Effect back2 = new StopEffect(2);
        Effect stop1 = new DeathEffect((float)0.5);
        Effect backStart = new BackStartEffect();
        Effect reverseEffect = new ReverseEffect();
        return new Inventory(new List<Effect> { back1, back2, stop1, backStart, reverseEffect});
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }
}
