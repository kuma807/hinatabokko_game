using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public static CardController Instance { get; private set; }
    private Card selectedCard;

    private void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン変更時にオブジェクトを保持
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HighlightCard(Card card)
    {
        // 既にハイライトされているカードがあれば、そのハイライトを消す
        if (selectedCard != null && selectedCard != card)
        {
            selectedCard.RemoveHighlight();
        }

        // 現在のカードをハイライト
        selectedCard = card;
        selectedCard.ApplyHighlight();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
