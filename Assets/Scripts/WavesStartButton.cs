using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesStartButton : MonoBehaviour
{
    public GameController gameController; 
    private void OnMouseDown()
    {
        gameController.SetWavesStart(true);
    }
}
