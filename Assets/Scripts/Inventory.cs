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

    public static Inventory StageSomeoneInventory()
    {
        Effect back3 = new BackEffect(10);
        Effect back4 = new BackEffect(10);
        Effect death1 = new DeathEffect(0.9999);
        Effect death2 = new DeathEffect(0.9999);
        Effect death3 = new DeathEffect(0.9999);
        Effect death4 = new DeathEffect(0.9999);
        Effect stop1 = new StopEffect(GameCalculater.TEN(10));
        Effect stop2 = new StopEffect(GameCalculater.TEN(10));
        Effect backstart = new BackStartEffect();
        Effect backstart2 = new ReverseEffect();
        return new Inventory(new List<Effect>{back3, back4, death1, death2, death3, death4, stop1, stop2, backstart, backstart2});
    }
}
