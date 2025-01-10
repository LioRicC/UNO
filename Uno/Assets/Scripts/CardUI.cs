using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CardUI : MonoBehaviour
{
    public TextMeshProUGUI numberText;  // Reference to UI text displaying the number
    public Image cardImage;  // Reference to the card's background image
    public Button cardButton; // Reference to the button for clicking

    public Sprite defaultCardSprite;  // Default card sprite in case of error
    private int cardNumber;
    private int cardColor;
    private CardGameManager gameManager;  // Reference to the game manager

    // Dictionary mapping number and color combinations to Sprites
    public Dictionary<(int number, int color), Sprite> cardGraphics;

    private void Start()
    {
        
        // Initialize the dictionary with sprites/materials assigned in the editor or code
        
        
        
        if (cardGraphics == null)
        {
            Debug.LogError("cardGraphics dictionary not initialized!");
        }
    }

    private void Awake()
    {
        InitializeCardGraphics();

    }

    private void InitializeCardGraphics()
    {
        cardGraphics = new Dictionary<(int number, int color), Sprite>()
        {
            // Red cards (1 to 9, 11 to 13)
            {(0, 1), Resources.Load<Sprite>("Red0")},
            {(1, 1), Resources.Load<Sprite>("Red1")},
            {(2, 1), Resources.Load<Sprite>("Red2")},
            {(3, 1), Resources.Load<Sprite>("Red3")},
            {(4, 1), Resources.Load<Sprite>("Red4")},
            {(5, 1), Resources.Load<Sprite>("Red5")},
            {(6, 1), Resources.Load<Sprite>("Red6")},
            {(7, 1), Resources.Load<Sprite>("Red7")},
            {(8, 1), Resources.Load<Sprite>("Red8")},
            {(9, 1), Resources.Load<Sprite>("Red9")},
            {(11, 1), Resources.Load<Sprite>("RedPlus2")},        // +2
            {(12, 1), Resources.Load<Sprite>("RedSkip")},         // Skip
            {(13, 1), Resources.Load<Sprite>("RedReverse")},      // Change direction
    
            // Blue cards (1 to 9, 11 to 13)
            {(0, 3), Resources.Load<Sprite>("Blue0")},
            {(1, 3), Resources.Load<Sprite>("Blue1")},
            {(2, 3), Resources.Load<Sprite>("Blue2")},
            {(3, 3), Resources.Load<Sprite>("Blue3")},
            {(4, 3), Resources.Load<Sprite>("Blue4")},
            {(5, 3), Resources.Load<Sprite>("Blue5")},
            {(6, 3), Resources.Load<Sprite>("Blue6")},
            {(7, 3), Resources.Load<Sprite>("Blue7")},
            {(8, 3), Resources.Load<Sprite>("Blue8")},
            {(9, 3), Resources.Load<Sprite>("Blue9")},
            {(11, 3), Resources.Load<Sprite>("BluePlus2")},       // +2
            {(12, 3), Resources.Load<Sprite>("BlueSkip")},        // Skip
            {(13, 3), Resources.Load<Sprite>("BlueReverse")},     // Change direction

            // Yellow cards (1 to 9, 11 to 13)
            {(0, 4), Resources.Load<Sprite>("Yellow0")},
            {(1, 4), Resources.Load<Sprite>("Yellow1")},
            {(2, 4), Resources.Load<Sprite>("Yellow2")},
            {(3, 4), Resources.Load<Sprite>("Yellow3")},
            {(4, 4), Resources.Load<Sprite>("Yellow4")},
            {(5, 4), Resources.Load<Sprite>("Yellow5")},
            {(6, 4), Resources.Load<Sprite>("Yellow6")},
            {(7, 4), Resources.Load<Sprite>("Yellow7")},
            {(8, 4), Resources.Load<Sprite>("Yellow8")},
            {(9, 4), Resources.Load<Sprite>("Yellow9")},
            {(11, 4), Resources.Load<Sprite>("YellowPlus2")},     // +2
            {(12, 4), Resources.Load<Sprite>("YellowSkip")},      // Skip
            {(13, 4), Resources.Load<Sprite>("YellowReverse")},   // Change direction

            // Green cards (1 to 9, 11 to 13)
            {(0, 2), Resources.Load<Sprite>("Green0")},
            {(1, 2), Resources.Load<Sprite>("Green1")},
            {(2, 2), Resources.Load<Sprite>("Green2")},
            {(3, 2), Resources.Load<Sprite>("Green3")},
            {(4, 2), Resources.Load<Sprite>("Green4")},
            {(5, 2), Resources.Load<Sprite>("Green5")},
            {(6, 2), Resources.Load<Sprite>("Green6")},
            {(7, 2), Resources.Load<Sprite>("Green7")},
            {(8, 2), Resources.Load<Sprite>("Green8")},
            {(9, 2), Resources.Load<Sprite>("Green9")},
            {(11, 2), Resources.Load<Sprite>("GreenPlus2")},      // +2
            {(12, 2), Resources.Load<Sprite>("GreenSkip")},       // Skip
            {(13, 2), Resources.Load<Sprite>("GreenReverse")},    // Change direction

            // Wild cards
            {(10, 0), Resources.Load<Sprite>("ChooseColor")}, // Wild choose color
            {(15, 0), Resources.Load<Sprite>("Plus4")},       // +4 and choose color

            // Special wild color cards (14)
            {(14, 1), Resources.Load<Sprite>("Red")},
            {(14, 2), Resources.Load<Sprite>("Green")},
            {(14, 3), Resources.Load<Sprite>("Blue")},
            {(14, 4), Resources.Load<Sprite>("Yellow")},

        };
    }

    // Update the card's display with the correct number and color, and set up its interaction
    public void UpdateCard(int number, int color, CardGameManager manager)

    {

        if (numberText == null || cardImage == null || cardButton == null)
        {
            Debug.LogError("One or more UI components are not assigned in the Inspector.");
            return;
        }
        if (manager == null)
        {
            Debug.LogError("CardGameManager is null when updating card.");
            return;  // Prevent further execution if null
        }
        Debug.Log("Updating card: " + number + ", color: " + color);

        if (cardGraphics == null)
        {
            Debug.LogError("cardGraphics is null.");
        }

        if (numberText == null)
        {
            Debug.LogError("numberText is null.");
        }

        if (cardImage == null)
        {
            Debug.LogError("cardImage is null.");
        }

        if (cardButton == null)
        {
            Debug.LogError("cardButton is null.");
        }

        if (manager == null)
        {
            Debug.LogError("CardGameManager is null.");
            return;
        }
        Debug.Log("UpdateCard executed");

        cardNumber = number;
        cardColor = color;
        gameManager = manager;

        // Set card graphics based on number and color
        if (cardGraphics.TryGetValue((number, color), out Sprite cardSprite))
        {
            cardImage.sprite = cardSprite;
            numberText.text = "";  // Optionally hide the number since the sprite shows it
        }
        else
        {
            Debug.LogWarning("No sprite found for card combination! Using default.");
            cardImage.sprite = defaultCardSprite;  // Use a default sprite if no match is found
            numberText.text = cardNumber.ToString();
        }

        // Clear previous listeners
        cardButton.onClick.RemoveAllListeners();

        // Attach the click event
        cardButton.onClick.AddListener(() => { Debug.Log("Button clicked!"); NotifyCardClicked(); });
    }

    // Get the card's color based on the integer value (can remove if no longer used)
    private Color GetColorFromValue(int colorValue)
    {
        switch (colorValue)
        {
            case 1: return Color.red;
            case 2: return Color.green;
            case 3: return Color.blue;
            case 4: return Color.yellow;
            default: return Color.white;  // Wild cards
        }
    }

    // Notify the game manager that the card was clicked
    public void NotifyCardClicked()
    {
        if (gameManager.GetComponent<CardGameManager>().playerHasPlayed == false)
        {
            Debug.Log("NotifyCardClicked executed");
            // Pass this card's information to the Game Manager
            gameManager.OnCardClicked((GetCardNumber(), GetCardColor()));
        }
    }

    

    // Getter methods for card number and color (optional, but useful)
    public int GetCardNumber() { return cardNumber; }
    public int GetCardColor() { return cardColor; }
}
