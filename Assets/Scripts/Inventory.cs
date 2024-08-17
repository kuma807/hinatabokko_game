using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GMTKGameJam.Scripts;

[System.Diagnostics.DebuggerDisplay("card: {card}")]
public class Inventory
{
    public Card card;

    public Inventory(Card _card)
    {
        card = _card;
    }

    public void DisplayInfo()
    {
        Debug.Log(this);
    }
}
