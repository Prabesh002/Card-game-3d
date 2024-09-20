using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefab : MonoBehaviour
{
    public TMP_Text cardName;

    public Image cardSpriteHolder;

    public Card card;



    public void OnMouseDown()
    {
        DOTweenManager doManager = GetComponent<DOTweenManager>();

        doManager.PlayAllAnimations();

        CardManager.cardManager.AddCard(card.cardName, gameObject);
        
    }
}
