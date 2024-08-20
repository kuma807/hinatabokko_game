using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



[System.Diagnostics.DebuggerDisplay("effect: {effect}")]
public class Card : MonoBehaviour
{
    private Color highlightColor;
    private Color originalColor;
    private SpriteRenderer spriteRenderer; 
    public Effect effect;
    public const int CardWidth = 120;
    public const int CardHeight = 180;
    public Card(Effect _effect)
    {
        effect = _effect;
    }

    private void Start()
    {
        highlightColor = Color.yellow;
        originalColor = new Color(0,0,0,0);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = originalColor;
    }

    public void DisplayInfo()
    {
        UnityEngine.Debug.Log(this);
    }

    private void OnMouseDown()
    {
        CardController.Instance.HighlightCard(this);
        if (GameController.Instance.tutorialState == TutorialState.beforeLeftClickCard)
        {
            GameController.Instance.tutorialState = TutorialState.beforeLeftClickCell;
            GameRenderer.Instance.DisplayTutorial("Left Click the cell to place trap.");
        }
    }
     public void ApplyHighlight()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = highlightColor; // ハイライト色に設定
        }
    }

    public void RemoveHighlight()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor; // 元の色に戻す
        }
    }

}
