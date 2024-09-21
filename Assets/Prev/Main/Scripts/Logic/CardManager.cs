using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour
{
    public static CardManager cardManager;

    public event Action<Card> OnCardSpawn;
    public event Action<Card> OnCardMatch;
    public event Action<Card> OnCardFail;

    public TMP_Text dd;

    public List<Card> allCards;
    public List<Transform> spawnPositions;

    [SerializeField] List<string> selectedCards = new List<string>();

    public List<GameObject> all = new();
    List<GameObject> gameObjects;

    public GameObject gameOverPanel;

    void Awake()
    {
        if (cardManager != null && cardManager != this)
        {
            Destroy(this);
        }
        else
        {
            cardManager = this;
        }


        gameObjects = new List<GameObject>();
    }

    void LateUpdate()
    {
        RefreshCardList();
        if (all.Count == 0)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(false);
        }
    }

    public void playa()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void AddCard(string cardName, GameObject cardObject)
    {
        if (selectedCards.Count < 2)
        {
            selectedCards.Add(cardName);
            Debug.Log(cardObject.name + " added to selected cards");

            GameObject clone = cardObject;
            gameObjects.Add(clone);

            Debug.Log("Card added");
        }

        if (selectedCards.Count == 2)
        {
            gameObjects.Add(cardObject);
            OnCardDraw(selectedCards, gameObjects);
        }
    }

    void OnCardDraw(List<string> ids, List<GameObject> cards)
    {
        if (ids[0] == ids[1])
        {
            // Cards match
            foreach (GameObject card in cards)
            {
                Destroy(card.gameObject);
                dd.text = "Cards match!";
                //OnCardMatch?.Invoke(); Didnt had time to properly imoplement,
            }
            selectedCards.Clear();
        }
        else
        {
            // Cards don't match
            Debug.Log("Cards don't match!");
            dd.text = "Cards don't match!";
            //OnCardFail?.Invoke(); Didnt had time to properly imoplement,

            selectedCards.Clear();
            foreach (GameObject go in gameObjects)
            {
                Destroy(go, 0.4f);

            }

          
            gameObjects.Clear();

          
            RefreshCardList();

           
            if (all.Count > 0)
            {
                CardInstantiateManager.inst.ShuffleCardsAfterSpawn();
            }
            else
            {
               
                ShowGameOverPanel();
            }
        }
    }

  
    void RefreshCardList()
    {
        all.RemoveAll(card => card == null); 
    }


    void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
