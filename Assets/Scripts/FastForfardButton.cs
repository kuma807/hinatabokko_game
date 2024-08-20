using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastForwardButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameController.Instance.FastForward();
    }
}
