using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellScript : MonoBehaviour
{
    public Card selectedCard;
    private GameObject obj;
    private SpriteRenderer spriteRenderer;
    private Card usedCard;
    private GameObject popup;
    private EffectIcons effectIcons;
    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("CardController");
        popup = transform.Find("Popup").gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();
        effectIcons = GameObject.Find("EffectIcons").GetComponent<EffectIcons>();
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
        CardController cardController = obj.GetComponent<CardController>();
        selectedCard = cardController.selectedCard;
        if (selectedCard != null)
        {
            usedCard = selectedCard;
            UnityEngine.Debug.Log(usedCard.effect);
            cardController.RemoveSelect();
            selectedCard.RemoveHighlight();
            //selectedCard.gameObject.GetComponentInParent<CanvasRenderer>();
            selectedCard.transform.parent.gameObject.SetActive(false);
            spriteRenderer.sprite = effectIcons.GetSpriteById(usedCard.effect.id);
        }
        
    }
    private void OnRightClick()
    {
        Debug.Log("right clicked");
        if (usedCard != null)
        {
            usedCard.transform.parent.gameObject.SetActive(true);
            spriteRenderer.sprite = null;
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
