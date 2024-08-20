using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{
    public Card selectedCard;
    public Sprite originaCellObject;
    private GameObject obj;
    private SpriteRenderer spriteRenderer;
    private Transform iconTransform;
    private Card usedCard;
    private GameObject popup;

    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("CardController");
        popup = transform.Find("Popup").gameObject;
        iconTransform = transform.Find("Icon");
        spriteRenderer = iconTransform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftClick();
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnRightClick();
        }
    }
    private void OnLeftClick()
    {
        if (GameController.Instance.gameState != GameState.preparing)
        {
            return;
        }
        if (spriteRenderer.sprite != originaCellObject)
        {
            return;
        }
        CardController cardController = obj.GetComponent<CardController>();
        selectedCard = cardController.selectedCard;
        if (selectedCard != null)
        {
            bool useCard = GameController.Instance.UseCardOnCell(selectedCard, gameObject);
            if (useCard)
            {
                if (GameController.Instance.tutorialState == TutorialState.beforeLeftClickCell)
                {
                    GameController.Instance.tutorialState = TutorialState.beforeRightClickCell;
                    GameRenderer.Instance.DisplayTutorial("Great! You successfully placed trap card on the board! Now try removing the trap, right Click the cell with trap to retrieve trap card.");
                }
                usedCard = selectedCard;
                cardController.RemoveSelect();
                selectedCard.RemoveHighlight();
                selectedCard.transform.parent.gameObject.SetActive(false);
            }
        }
    }
    private void OnRightClick()
    {
        if (GameController.Instance.gameState != GameState.preparing)
        {
            return;
        }
        if (spriteRenderer.sprite != originaCellObject)
        {
            if (GameController.Instance.tutorialState == TutorialState.beforeRightClickCell)
            {
                GameController.Instance.tutorialState = TutorialState.beforeClickStartWaves;
                GameRenderer.Instance.DisplayTutorial("Ok! Now you know how to place and remove trap! Press the play button when you are ready for enemy attack. The enemy will come from the black castle.");
            }
            usedCard.transform.parent.gameObject.SetActive(true);
            spriteRenderer.sprite = originaCellObject;
            iconTransform.localScale = new Vector3((float)1.0, (float)1.0, (float)1.0);
        }
    }

    public void OnMouseEnter()
    {
        popup.SetActive(true);
    }

    public void OnMouseExit()
    {
        popup.SetActive(false);
    }
}
