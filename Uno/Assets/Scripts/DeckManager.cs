using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private List<(int, int)> fullDeck = new List<(int, int)>();
    private List<(int, int)> discardPile = new List<(int, int)>();// Full UNO deck with real cards

    void Start()
    {
        InitializeDeck();
        ShuffleDeck();
    }

    // Initialize the deck with numbered cards and action cards based on the provided rules
    private void InitializeDeck()
    {
        fullDeck.Clear();

        // Add numbered and action cards for each color (1 = red, 2 = green, 3 = blue, 4 = yellow)
        for (int color = 1; color <= 4; color++)  // 1 = red, 2 = green, 3 = blue, 4 = yellow
        {
            // Add one '0' card for each color
            fullDeck.Add((0, color));

            // Add two copies of each number card from 1 to 9
            for (int number = 1; number <= 9; number++)
            {
                fullDeck.Add((number, color));
                fullDeck.Add((number, color));
            }

            // Add two Draw Two (11), Skip (12), and Reverse (13) cards for each color
            fullDeck.Add((11, color));  // Draw Two
            fullDeck.Add((11, color));  // Draw Two
            fullDeck.Add((12, color));  // Skip
            fullDeck.Add((12, color));  // Skip
            fullDeck.Add((13, color));  // Reverse
            fullDeck.Add((13, color));  // Reverse
        }

        // Add Wild (10, 0) and Wild Draw Four (15, 0) cards (four of each)
        for (int i = 0; i < 4; i++)
        {
            fullDeck.Add((10, 0)); // Wild - Choose Color
            fullDeck.Add((15, 0)); // Wild Draw Four
        }
    }

    // Shuffle the deck
    private void ShuffleDeck()
    {
        fullDeck = fullDeck.OrderBy(x => Random.value).ToList();
    }

    // Draw a hand of cards for the player (let's say 7 cards)
    public List<(int, int)> DrawPlayerHand()
    {
        List<(int, int)> hand = new List<(int, int)>();

        for (int i = 0; i < 7; i++)
        {
            hand.Add(fullDeck[0]);
            fullDeck.RemoveAt(0);
        }

        return hand;
    }

    // Draw the first card from the deck to be played
    // Draw the first card from the deck to be played
    public (int, int) DrawFirstCard()
    {
        (int, int) firstCard = fullDeck[0];

        // If the first card is a Wild (10, 0) or Wild Draw Four (15, 0), keep drawing until a valid card is found
        while (firstCard.Item1 == 10 || firstCard.Item1 == 15 || firstCard.Item1 == 11 || firstCard.Item1 == 12 || firstCard.Item1 == 13)
        {
            // Move the invalid card back to the deck (reshuffle if necessary)
            fullDeck.RemoveAt(0);
            fullDeck.Add(firstCard); // Optional: Add the card to the end of the deck

            // Draw a new card
            firstCard = fullDeck[0];
        }

        // Once a valid card is found, remove it from the deck
        fullDeck.RemoveAt(0);
        AddToDiscardPile(firstCard);
        return firstCard;
    }


    // Draw a card from the deck when the player clicks "Draw Card"
    
    public (int, int) DrawCardFromDeck()
    {
        if (fullDeck.Count == 0)
        {
           ReShuffleDeck();
        }

        (int, int) drawnCard = fullDeck[0];
        fullDeck.RemoveAt(0);
        return drawnCard;
    }

    public List<(int, int)> GetDeck()
    {
        return fullDeck;
    }
    public void ReShuffleDeck()
    {
        discardPile = discardPile.OrderBy(x => Random.value).ToList();

        // Add the shuffled discard pile back to the full deck
        fullDeck.AddRange(discardPile);

        // Clear the discard pile
        discardPile.Clear();

        Debug.Log("Deck reshuffled from discard pile.");
    }

    public void AddToDiscardPile((int, int) card)
    {
        discardPile.Add(card);
        Debug.Log($"discardpile: {string.Join(", ", discardPile.Select(c => $"({c.Item1}, {c.Item2})"))}");
    }
}
