using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


[System.Diagnostics.DebuggerDisplay("effect: {effect}")]
public class Card
{
    public Effect effect;

    public Card(Effect _effect)
    {
        effect = _effect;
    }

    public void DisplayInfo()
    {
        Debug.Log(this);
    }
}
