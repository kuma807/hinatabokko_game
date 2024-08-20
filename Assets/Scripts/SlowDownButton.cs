using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameController.Instance.SlowDown();
    }
}
