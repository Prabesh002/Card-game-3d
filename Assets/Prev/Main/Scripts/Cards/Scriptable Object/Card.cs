using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Card", menuName = "Cards/Card", order = 1)]
public class Card : ScriptableObject
{

    public string cardName;

    public string cardId;

    public Sprite icon;

    public GameObject CardPrefab;
}
