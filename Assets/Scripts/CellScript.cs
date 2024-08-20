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
            GameController.Instance.UseCardOnCell(selectedCard, gameObject);
            usedCard = selectedCard;
            cardController.RemoveSelect();
            selectedCard.RemoveHighlight();
            //selectedCard.gameObject.GetComponentInParent<CanvasRenderer>();
            selectedCard.transform.parent.gameObject.SetActive(false);
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
