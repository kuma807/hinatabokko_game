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
        Effect back1 = new BackEffect(1);
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

    public static Inventory TutorialInventory()
    {
        Effect back1 = new BackEffect(1);
        Effect back2 = new BackEffect(2);
        Effect back3 = new BackEffect(3);
        Effect backto = new BackStartEffect();
        return new Inventory(new List<Effect>{back1, back2, back3, backto});
    }

    public static Inventory Stage1Inventory()
    {
        Effect back1 = new BackEffect(1);
        Effect back2 = new StopEffect(2);
        Effect stop1 = new DeathEffect((float)0.5);
        Effect backStart = new BackStartEffect();
        Effect reverseEffect = new ReverseEffect();
        return new Inventory(new List<Effect> { back1, back2, stop1, backStart, reverseEffect});
    }
    public static Inventory Stage2Inventory()
    {
        Effect back1 = new BackEffect(1);
        Effect back2 = new StopEffect(2);
        Effect stop1 = new DeathEffect((float)0.5);
        Effect backStart = new BackStartEffect();
        Effect reverseEffect = new ReverseEffect();
        return new Inventory(new List<Effect> { back1, back2, stop1, backStart, reverseEffect});
    }

}
