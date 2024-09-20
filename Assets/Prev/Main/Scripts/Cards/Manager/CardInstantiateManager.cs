using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardInstantiateManager : MonoBehaviour
{
    public List<Transform> spawnPositions;
    private List<int> usedPositions = new List<int>();
    public List<GameObject> instantiatedCards = new List<GameObject>();

    public static CardInstantiateManager inst;

    void Start()
    {
        SpawnCards();
        inst = this;
    }

    void SpawnCards()
    {
        List<Card> cardsToSpawn = CardManager.cardManager.allCards;
        Shuffle(cardsToSpawn);
        
        // Create a list to hold two pairs of each card
        List<Card> cardsToSpawnWithPairs = new List<Card>();
        foreach (Card card in cardsToSpawn)
        {
            cardsToSpawnWithPairs.Add(card);  // First instance
            cardsToSpawnWithPairs.Add(card);  // Second instance (for the pair)
        }

        Shuffle(cardsToSpawnWithPairs);  // Shuffle the pairs

        // Instantiate all cards
        for (int i = 0; i < cardsToSpawnWithPairs.Count; i++)
        {
            int randomIndex = GetEmptyPosition();
            if (randomIndex == -1)
            {
                Debug.LogError("No empty position found!");
                break;
            }

            Transform spawnPosition = spawnPositions[randomIndex];
            GameObject cardObject = Instantiate(cardsToSpawnWithPairs[i].CardPrefab, spawnPosition.transform);
            usedPositions.Add(randomIndex);

            CardPrefab cardPrefabScript = cardObject.GetComponent<CardPrefab>();
            if (cardPrefabScript != null)
            {
                cardPrefabScript.card = cardsToSpawnWithPairs[i];
                cardPrefabScript.cardName.text = cardsToSpawnWithPairs[i].cardName;
                if (cardPrefabScript.cardSpriteHolder != null && cardsToSpawnWithPairs[i].icon != null)
                {
                    cardPrefabScript.cardSpriteHolder.sprite = cardsToSpawnWithPairs[i].icon;
                }
            }

            cardObject.transform.DOMove(spawnPosition.position, 1f).SetEase(Ease.InOutBack);
            instantiatedCards.Add(cardObject);  // Add instantiated card to list
        }

        // Shuffle cards after instantiation
        Invoke("ShuffleCardsAfterSpawn", 1.5f);  // Delay to ensure all cards are placed
    }

    public void Shuffle(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Card temp = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    int GetEmptyPosition()
    {
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if (!usedPositions.Contains(i))
            {
                return i;
            }
        }

        return -1;
    }

    public void ShuffleCardsAfterSpawn()
    {
          CardManager.cardManager.all = CardInstantiateManager.inst.instantiatedCards;
        // Shuffle the cards between positions
        List<int> availablePositions = new List<int>();
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            availablePositions.Add(i);
        }

        ShufflePositions(availablePositions);

        // Move the instantiated cards to their new random positions
        for (int i = 0; i < instantiatedCards.Count; i++)
        {
            int randomIndex = availablePositions[i];
            Transform newPosition = spawnPositions[randomIndex];
            instantiatedCards[i].transform.DOMove(newPosition.position, 1f).SetEase(Ease.InOutBack);
        }

        // Clear used positions for future spawns
        usedPositions.Clear();
    }

    void ShufflePositions(List<int> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            int temp = positions[i];
            int randomIndex = Random.Range(i, positions.Count);
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }
    }
}
