using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Xml;
using System.Xml.Linq;

public class CardGameManager : MonoBehaviour
{
    public List<GameObject> playerHandUI = new List<GameObject>(); // Stores card UI elements
    public List<(int, int)> playerHand = new List<(int, int)>();   // Player's hand (tuples of (number, color))
    public List<(int, int)> aiHand1;
    public List<(int, int)> aiHand2;
    public List<(int, int)> aiHand3;
    public List<(int, int)> deck;
    public (int, int) lastCardPlayed;  // Last card that was played

    private int gameDirection = 1;  // 1 for clockwise, -1 for counter-clockwise
    private int turnIndex = 0;
    public bool playerHasPlayed = false;

    private int drawCardClickCount = 0;

    public GameObject CardPrefab; // Card prefab for UI
    public GameObject CardPrefab2;

    public Transform PlayerHandTransform; // UI Transform to place cards
    public Button drawCardButton;  // Reference to the "Draw Card" button

    public GameObject lastCardPlayedUI;  // Store the last card played in the UI
    public Transform LastCardPlayedTransform;  // Position where the last card will be displayed

    public GameObject imageMoverManager;
    public RectTransform imageTransform;
    public RectTransform imageTransform2;
    public RectTransform imageTransform3;
    public RectTransform imageTransform4;
    public RectTransform imageTransform5;

    private DeckManager deckManager;

    public GameObject winManager;

    public GameObject background;
    public bool insaneModeOn;
    public bool impossibleModeOn;
    public GameObject RedImage;
    public TextMeshProUGUI RedImageText;

    public TextMeshProUGUI cardCount0;
    public TextMeshProUGUI cardCount1;
    public TextMeshProUGUI cardCount2;
    public TextMeshProUGUI cardCount3;
    public GameObject colManagerObj;
    public GameObject modelManager;

    public TextMeshProUGUI name0GUI;
    public TextMeshProUGUI name1GUI;
    public TextMeshProUGUI name2GUI;
    public TextMeshProUGUI name3GUI;
    public GameObject nameManager;

    public List<GameObject> animationLocations = new List<GameObject>();
    public GameObject location0;
    public GameObject location1;
    public GameObject location2;
    public GameObject location3;
    public GameObject location02;
    public GameObject location12;
    public GameObject location22;
    public GameObject location32;
    public GameObject location03;
    public GameObject location13;
    public GameObject location23;
    public GameObject location33;

    public Vector2 playerHandVector = new Vector2(-152.76f, -208.12f);
    public Vector2 ai1HandVector = new Vector2(-793.4902f, 3.049976f);
    public Vector2 ai2HandVector = new Vector2(-149.43f, 448.2801f);
    public Vector2 ai3HandVector = new Vector2(764.0499f, -1.39997f);

    public List<Vector2> handVectors;
    public (int, int) blueZero = (0, 3);
    public (int, int) yellowChangeDirection = (13, 4);

    public bool stageCanDraw=true;

    







    void Start()
    {
        impossibleModeOn = false;
        insaneModeOn = false;
        deckManager = FindObjectOfType<DeckManager>();
        StartCoroutine(WaitForModelInitialization());

        animationLocations.Add(location0);
        animationLocations.Add(location1);
        animationLocations.Add(location2);
        animationLocations.Add(location3);
        animationLocations.Add(location02);
        animationLocations.Add(location12);
        animationLocations.Add(location22);
        animationLocations.Add(location32);
        animationLocations.Add(location03);
        animationLocations.Add(location13);
        animationLocations.Add(location23);
        animationLocations.Add(location33);

        handVectors = new List<Vector2>
        {
            playerHandVector,
            ai1HandVector,
            ai2HandVector,
            ai3HandVector
        };

        //imageMoverManager.GetComponent<MoveUIImage>().;


        if (drawCardButton != null)
        {
            drawCardButton.onClick.RemoveAllListeners();
            drawCardButton.onClick.AddListener(OnDrawCardClicked);
        }

        lastCardPlayed = deckManager.DrawFirstCard();
        Debug.Log($"passing to CreateLastCardPlayedUI: {lastCardPlayed} ");
        CreateLastCardPlayedUI(lastCardPlayed);



    }

    private IEnumerator WaitForModelInitialization()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Model initialized");

        playerHand = deckManager.DrawPlayerHand();
        //playerHand.Add(blueZero);
        

        aiHand1 = deckManager.DrawPlayerHand();
        aiHand2 = deckManager.DrawPlayerHand();
        aiHand3 = deckManager.DrawPlayerHand();
        deck = deckManager.GetDeck();
        Debug.Log($"Playerhand: {FormatHand(playerHand)} ");
        Debug.Log($"ai1hand: {FormatHand(aiHand1)} ");
        Debug.Log($"ai2hand: {FormatHand(aiHand2)} ");
        Debug.Log($"ai3hand: {FormatHand(aiHand3)} ");
        Debug.Log($"Deck: {deck.Count} ");
        Debug.Log($"lastCardPlayed: {lastCardPlayed} ");

        CreatePlayerHandUI(playerHand);
        StartCoroutine(Turn());
    }

    private IEnumerator Turn()
    {
        while (true)
        {
            // Handle turns based on the current player index
            switch (turnIndex)
            {
                case 0: // Player's turn
                    
                    name0GUI.fontStyle = FontStyles.Underline;
                    name0GUI.color = Color.red;
                    name1GUI.fontStyle = FontStyles.Normal;
                    name1GUI.color = Color.white;
                    name2GUI.fontStyle = FontStyles.Normal;
                    name2GUI.color = Color.white;
                    name3GUI.fontStyle = FontStyles.Normal;
                    name3GUI.color = Color.white;
                    stageCanDraw=true;
                    


                    yield return StartCoroutine(PlayerTurn());
                    break;
                case 1: // AI1's turn
                    name0GUI.fontStyle = FontStyles.Normal;
                    name0GUI.color = Color.white;
                    name1GUI.fontStyle = FontStyles.Underline;
                    name1GUI.color = Color.green;
                    name2GUI.fontStyle = FontStyles.Normal;
                    name2GUI.color = Color.white;
                    name3GUI.fontStyle = FontStyles.Normal;
                    name3GUI.color = Color.white;
                    
                    yield return new WaitForSeconds(2f);
                    yield return StartCoroutine(AITurn(aiHand1, "AI1"));
                    yield return new WaitForSeconds(1f);



                    break;
                case 2: // AI2's turn
                    name0GUI.fontStyle = FontStyles.Normal;
                    name0GUI.color = Color.white;
                    name1GUI.fontStyle = FontStyles.Normal;
                    name1GUI.color = Color.white;
                    name2GUI.fontStyle = FontStyles.Underline;
                    name2GUI.color = Color.blue;
                    name3GUI.fontStyle = FontStyles.Normal;
                    name3GUI.color = Color.white;
                    
                    yield return new WaitForSeconds(2f);
                    yield return StartCoroutine(AITurn(aiHand2, "AI2"));
                    yield return new WaitForSeconds(1f);
                    break;
                case 3: // AI3's turn
                    name0GUI.fontStyle = FontStyles.Normal;
                    name0GUI.color = Color.white;
                    name1GUI.fontStyle = FontStyles.Normal;
                    name1GUI.color = Color.white;
                    name2GUI.fontStyle = FontStyles.Normal;
                    name2GUI.color=Color.white;
                    name3GUI.fontStyle = FontStyles.Underline;
                    name3GUI.color = Color.yellow;
                    
                    yield return new WaitForSeconds(2f);
                    yield return StartCoroutine(AITurn(aiHand3, "AI3"));
                    yield return new WaitForSeconds(1f);
                    break;
            }

            // Check for win conditions after each turn
            if (CheckForWin())
            {
                Debug.Log("Game Over! We have a winner!");
                StopAllCoroutines();
                break;
            }

            // Update turn index based on game direction
            UpdateTurn();

            // Add delay to simulate AI thinking time or wait for the next turn
            yield return new WaitForSeconds(0.5f);
        }
    }
    

    private void UpdateTurn()
    {
        // Move to the next player based on the game direction
        turnIndex = (turnIndex + gameDirection + 4) % 4;
    }

    private bool CheckForWin()
    {
        if(insaneModeOn==false&&impossibleModeOn==false)
        {
            UpdateHandCounts();
            if (playerHand.Count == 0)
            {
                Debug.Log("Player wins!");

                winManager.GetComponent<WinScript>().Winner("You");

                return true;
            }
            else if (aiHand1.Count == 0)
            {
                Debug.Log("AI1 wins!");
                string namewinner = nameManager.GetComponent<JSONLoader>().name1;
                winManager.GetComponent<WinScript>().Winner(namewinner);

                return true;
            }
            else if (aiHand2.Count == 0)
            {
                Debug.Log("AI2 wins!");
                string namewinner = nameManager.GetComponent<JSONLoader>().name2;
                winManager.GetComponent<WinScript>().Winner(namewinner);
                return true;
            }
            else if (aiHand3.Count == 0)
            {
                Debug.Log("AI3 wins!");
                string namewinner = nameManager.GetComponent<JSONLoader>().name3;
                winManager.GetComponent<WinScript>().Winner(namewinner);
                return true;
            }

            return false;
        }
        
        else
        {
            UpdateHandCounts();
            if (playerHand.Count == 0)
            {
                Debug.Log("Player wins!");

                winManager.GetComponent<WinScript>().WinnerInsanePlayer();

                return true;
            }
            
            else if (aiHand1.Count == 0)
            {

                winManager.GetComponent<WinScript>().WinnerInsaneAi();

                return true;
            }
            else if (aiHand2.Count == 0)
            {
                winManager.GetComponent<WinScript>().WinnerInsaneAi();

                return true;
            }
            else if (aiHand3.Count == 0)
            {
                winManager.GetComponent<WinScript>().WinnerInsaneAi();

                return true;
            }

            return false;
        }
        
    }

    private IEnumerator AITurn(List<(int, int)> aiHand, string aiName)
    {
        Debug.Log($"{aiName}'s turn.");

        ModelManager modelManager = GetComponent<ModelManager>();
        if (modelManager != null)
        {
            int chosenCardIndex = modelManager.runmodel1(aiHand, lastCardPlayed);
            chosenCardIndex--;
            if (chosenCardIndex == -1)
            {
                (int, int) drawnCard = deckManager.DrawCardFromDeck();
                Debug.Log("drew");
                if (drawnCard != (-1, -1))
                {
                    if (aiName == "AI1")
                    {
                        imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromDeckToHand(ai1HandVector,0.5f, imageTransform);
                    }
                    if (aiName == "AI2")
                    {
                        imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromDeckToHand(ai2HandVector,0.5f,imageTransform);
                    }
                    if (aiName == "AI3")
                    {
                        imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromDeckToHand(ai3HandVector, 0.5f, imageTransform);
                    }

                    
                    if (IsValidMove(drawnCard,lastCardPlayed)){
                        lastCardPlayed=drawnCard;
                        
                        deckManager.AddToDiscardPile(lastCardPlayed);
                        yield return new WaitForSeconds(1f);
                        UpdateHandCounts();

                        if (aiName == "AI1")
                        {
                            imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromHandToField(ai1HandVector, 0.5f, imageTransform5);
                        }
                        if (aiName == "AI2")
                        {
                            imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromHandToField(ai2HandVector, 0.5f, imageTransform5);
                        }
                        if (aiName == "AI3")
                        {
                            imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromHandToField(ai3HandVector, 0.5f, imageTransform5);
                        }
                        
                        yield return new WaitForSeconds(0.5f);

                        CreateLastCardPlayedUI(lastCardPlayed);

                        if (IsSpecialCard(lastCardPlayed))
                        {
                            HandleSpecialCardAI(lastCardPlayed, aiHand);
                        }
                    }
                    else{
                        aiHand.Add(drawnCard);
                    }
                    

                    



                }
                else
                {
                    Debug.Log("No more cards in the deck to draw!");
                }
            }

            if (chosenCardIndex >= 0 && chosenCardIndex < aiHand.Count)
            {
                (int, int) chosenCard = aiHand[chosenCardIndex];
                Debug.Log($"{aiName} played: ({chosenCard.Item1}, {chosenCard.Item2})");

                // Update last card played and remove the card from the AI's hand
                lastCardPlayed = chosenCard;
                deckManager.AddToDiscardPile(lastCardPlayed);
                aiHand.RemoveAt(chosenCardIndex);

                if (aiName == "AI1")
                {
                    imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromHandToField(ai1HandVector, 0.5f, imageTransform5);
                }
                if (aiName == "AI2")
                {
                    imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromHandToField(ai2HandVector, 0.5f, imageTransform5);
                }
                if (aiName == "AI3")
                {
                    imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromHandToField(ai3HandVector, 0.5f, imageTransform5);
                }
                
                yield return new WaitForSeconds(0.5f);

                CreateLastCardPlayedUI(lastCardPlayed);

                if (CheckForWin())
                {
                    Debug.Log("Game Over! We have a winner!");
                    StopAllCoroutines();

                }


                // Check for special cards like Reverse or Wild
                if (IsSpecialCard(chosenCard))
                {
                    HandleSpecialCardAI(chosenCard, aiHand);
                }



            }

            cardCount0.text = playerHand.Count.ToString();
            cardCount1.text = aiHand1.Count.ToString();
            cardCount2.text = aiHand2.Count.ToString();
            cardCount3.text = aiHand3.Count.ToString();

        }
        else
        {
            Debug.LogError("ModelManager component not found!");
        }

        yield return new WaitForSeconds(2f);  // Add some delay for AI's "thinking"
    }

    


    private IEnumerator PlayerTurn()
    {
        Debug.Log("Player's turn. Waiting for player to make a move...");
        playerHasPlayed = false;
        

        // Wait until the player plays a valid card
        while (!playerHasPlayed)
        {
            yield return null;  // Keep waiting until player plays
        }
        UpdateHandCounts();
        Debug.Log("Player has made a move.");
        
    }
    void UpdateHandCounts()
    {
        Debug.Log("Updated playerhand from " + cardCount0.text + " to " + playerHand.Count.ToString());
        cardCount0.text = playerHand.Count.ToString();
        
        cardCount1.text = aiHand1.Count.ToString();
        cardCount2.text = aiHand2.Count.ToString();
        cardCount3.text = aiHand3.Count.ToString();
    }

    public void OnCardClicked((int, int) card)
    {
        Debug.Log($"Card clicked with number: {card.Item1} and color: {card.Item2}");
        Debug.Log($"lastCardPlayed: {lastCardPlayed} ");

        if (IsValidMove(card, lastCardPlayed))
        {
            if (lastCardPlayed != (15, 0) && lastCardPlayed != (10, 0))
            {
                //imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromHandToField(playerHandVector, 0.5f, imageTransform);

                playerHand.Remove(card);            // Remove card from player's hand
                lastCardPlayed = card;
                deckManager.AddToDiscardPile(lastCardPlayed);// Update last card played
                CreateLastCardPlayedUI(lastCardPlayed);  // Update the last card UI with the new card played

                Debug.Log($"Player played: ({card.Item1}, {card.Item2}). Cards left: {playerHand.Count}");

                /*if (playerHand.Count == 0)
                {
                    Debug.Log("Player wins!");
                    StopAllCoroutines();
                }*/

                if (IsSpecialCard(card))
                {
                    HandleSpecialCardHuman(card);
                }
                else
                {
                    playerHasPlayed = true;
                }
                /*{
                    int chosenColor = GetPlayerChosenColor();
                    lastCardPlayed = (lastCardPlayed.Item1, chosenColor); // Update with chosen color
                    Debug.Log($"Player chose new color: {chosenColor}");
                }*/
                RemoveCardFromPlayerHandUI(card);

            }


        }
        else
        {
            Debug.Log($"card: {card} ");
            Debug.Log("Invalid move!");
        }
    }


    
        

    public void OnDrawCardClicked()
    {
        foreach ((int,int) card in playerHand)
        {
            if (IsValidMove(card, lastCardPlayed))
            {
                stageCanDraw = false;
                break;
            }
        }

        if (playerHasPlayed == false&&stageCanDraw==true)
        {
            drawCardClickCount++;
            Debug.Log($"Draw Card button clicked! Count: {drawCardClickCount}");

            (int, int) drawnCard = deckManager.DrawCardFromDeck();

            if (drawnCard != (-1, -1))
            {
                playerHand.Add(drawnCard);
                UpdateHandCounts();
                Debug.Log($"Player drew a card: ({drawnCard.Item1}, {drawnCard.Item2}). Cards in hand: {playerHand.Count}");
                imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromDeckToHand(playerHandVector, 0.5f, imageTransform);
                AddCardToPlayerHandUI(drawnCard);
                if(!IsValidMove(drawnCard,lastCardPlayed)){
                    playerHasPlayed = true;
                }
                else{
                    stageCanDraw=false;
                }
                
            }
            else
            {
                Debug.Log("No more cards in the deck to draw!");
            }
            
        }

    }

    private void CreatePlayerHandUI(List<(int, int)> playerHand)
    {
        foreach (var card in playerHand)
        {
            AddCardToPlayerHandUI(card);
        }
    }

    void AddCardToPlayerHandUI((int, int) card)
    {
        // Instantiate the card prefab and parent it to PlayerHandTransform
        GameObject cardObject = Instantiate(CardPrefab, PlayerHandTransform);
        CardUI cardUI = cardObject.GetComponent<CardUI>();

        // Update the card UI with the card's data
        if (cardUI != null)
        {
            cardUI.UpdateCard(card.Item1, card.Item2, this);  // Pass reference to Game Manager
        }
        else
        {
            Debug.LogError("CardUI component not found on instantiated card object!");
        }

        // Add the card to the player's hand UI list
        playerHandUI.Add(cardObject);

        // Reposition all cards in the player's hand to apply dynamic overlap
        RepositionPlayerHandUI();
    }

    void RepositionPlayerHandUI()
    {
        int cardCount = playerHandUI.Count;

        // Define how much we want cards to overlap more gradually as the number of cards increases
        float baseSpacing = 500f;  // Maximum space between the cards when there are fewer cards
        float minSpacing = 10f;    // Minimum space between the cards when there are many cards

        // Calculate the overlap reduction based on the number of cards
        float overlapAmount = Mathf.Lerp(baseSpacing, minSpacing, (float)(cardCount - 1) / (float)cardCount);

        // Loop through all cards and adjust their positions
        for (int i = 0; i < cardCount; i++)
        {
            // Get the RectTransform of each card
            RectTransform cardRectTransform = playerHandUI[i].GetComponent<RectTransform>();

            if (cardRectTransform != null)
            {
                // Reposition cards with gentle overlap
                Vector3 cardPosition = new Vector3(i * overlapAmount, 0, 0);
                cardRectTransform.anchoredPosition = cardPosition;
            }
            else
            {
                Debug.LogError("RectTransform component not found on card object!");
            }
        }
    }


    private void RemoveCardFromPlayerHandUI((int, int) card)
    {
        GameObject cardToRemove = playerHandUI.Find(cardUI =>
        {
            CardUI ui = cardUI.GetComponent<CardUI>();
            return ui.GetCardNumber() == card.Item1 && ui.GetCardColor() == card.Item2;
        });

        if (cardToRemove != null)
        {
            playerHandUI.Remove(cardToRemove);
            Destroy(cardToRemove);
        }
        RepositionPlayerHandUI();
    }





    // Method to create or update the "last card played" UI
    void CreateLastCardPlayedUI((int, int) lastCard)
    {
        Debug.Log("CreatelastCardplayedUI called");

        // Destroy the previous last card UI if it exists
        if (lastCardPlayedUI != null)
        {
            Destroy(lastCardPlayedUI);
        }

        // Instantiate a new card prefab and position it
        lastCardPlayedUI = Instantiate(CardPrefab2, LastCardPlayedTransform);

        // Get the CardUI component
        CardUI cardUI = lastCardPlayedUI.GetComponent<CardUI>();

        // Update the card UI with the last card played
        if (cardUI != null)
        {
            cardUI.UpdateCard(lastCard.Item1, lastCard.Item2, this);
        }
        else
        {
            Debug.LogError("CardUI component not found on instantiated card object!");
        }

        // Get the Button component from the instantiated card prefab or its children and disable it
        Button cardButton = lastCardPlayedUI.GetComponentInChildren<Button>();
        if (cardButton != null)
        {
            cardButton.interactable = false;
            Debug.Log("Button interactable set to false.");
        }
        else
        {
            Debug.LogError("Button component not found on instantiated card object or its children!");
        }
    }



    private bool IsValidMove((int, int) card, (int, int) lastCardPlayed)
    {
        return (card.Item2 == 0 || lastCardPlayed.Item2 == 0 ||
                card.Item1 == lastCardPlayed.Item1 ||
                card.Item2 == lastCardPlayed.Item2);
    }

    private bool IsSpecialCard((int, int) card)
    {
        return card.Item1 >= 10;
    }

    private void HandleSpecialCardHuman((int, int) card)
    {

        
        switch (card.Item1)
        {
            case 10: // Choose Color
                Debug.Log("Choose color card played! Player or AI must select a color.");
                GetPlayerChosenColor(10); // Assuming AI will select color randomly for now
                


                break;

            case 11: // Draw 2 and skip next player's turn
                Debug.Log("Draw 2 card played! The next player will draw 2 cards and lose their turn.");
                SkipAndDrawCards(2);
                playerHasPlayed = true;
                break;

            case 12: // Skip next player's turn
                Debug.Log("Skip card played! The next player will lose their turn.");
                SkipNextPlayer();
                playerHasPlayed = true;
                break;

            case 13: // Reverse direction
                Debug.Log("Reverse card played! The game direction is reversed.");
                ReverseGameDirection();
                playerHasPlayed = true;
                break;

            case 15: // Draw 4, skip turn, and choose color
                Debug.Log("Draw 4 card played! The next player will draw 4 cards, lose their turn, and a color will be chosen.");
                GetPlayerChosenColor(15);
                
                 // Again, assume random for AI

                break;

            default:
                Debug.Log("No special card effect.");
                break;
        }
    }

    private void HandleSpecialCardAI((int, int) card, List<(int, int)> aiHand)
    {
        
        switch (card.Item1)
        {
            case 10: // Choose Color
                Debug.Log("Choose color card played! Player or AI must select a color.");
                lastCardPlayed = modelManager.GetComponent<ModelManager>().runmodel2(aiHand);
                StartCoroutine(CallFunctionAfterDelay(1f, lastCardPlayed));
                

                // Assuming AI will select color randomly for now

                break;

            case 11: // Draw 2 and skip next player's turn
                Debug.Log("Draw 2 card played! The next player will draw 2 cards and lose their turn.");
                SkipAndDrawCards(2);
                break;

            case 12: // Skip next player's turn
                Debug.Log("Skip card played! The next player will lose their turn.");
                SkipNextPlayer();
                break;

            case 13: // Reverse direction
                Debug.Log("Reverse card played! The game direction is reversed.");
                ReverseGameDirection();
                break;

            case 15: // Draw 4, skip turn, and choose color
                Debug.Log("Draw 4 card played! The next player will draw 4 cards, lose their turn, and a color will be chosen.");
                lastCardPlayed = modelManager.GetComponent<ModelManager>().runmodel2(aiHand);
                StartCoroutine(CallFunctionAfterDelay(1f, lastCardPlayed));

                SkipAndDrawCards(4);
                // Again, assume random for AI

                break;

            default:
                Debug.Log("No special card effect.");
                break;
        }
    }

    IEnumerator CallFunctionAfterDelay(float delay, (int,int) card)
    {
        // Wait for the specified delay (1 second in this case)
        yield return new WaitForSeconds(delay);

        // Call the function with the argument
        CreateLastCardPlayedUI(card);
    }

    private void GetPlayerChosenColor(int whatChecking)
    {
        colManagerObj.GetComponent<ColorManager>().ActivateColorGUI();
        
        StartCoroutine(BooleanChecker(whatChecking));



    }


    public IEnumerator BooleanChecker(int whatChecking)
    {
        while (!colManagerObj.GetComponent<ColorManager>().playerHasChosenCol)
        {
            yield return null;  // Keep waiting until player plays
        }

        (int, int) chosenColor = colManagerObj.GetComponent<ColorManager>().chosenCol;

        lastCardPlayed = chosenColor;
        Debug.Log($"New color chosen: {chosenColor}");
        colManagerObj.GetComponent<ColorManager>().colorGui.SetActive(false);
        CreateLastCardPlayedUI(lastCardPlayed);
        if (whatChecking == 15) 
        {
            SkipAndDrawCards(4);
        }
        playerHasPlayed = true;

        yield return chosenColor;
    }
    




    private void ReverseGameDirection()
    {
        gameDirection *= -1;  // Flip the direction
        Debug.Log("Game direction reversed!");
    }

    // Skip the next player's turn
    private void SkipNextPlayer()
    {
        // Move the turn index forward by one in the current game direction, essentially skipping the next player's turn
        UpdateTurn(); // Move to the next player's turn
        Debug.Log($"Player at index {turnIndex} has been skipped.");
        CallFunctionOnLocationSkip(animationLocations[turnIndex]);

    }

    void CallFunctionOnLocationSkip(GameObject location)
    {
        // Example function call - replace this with what you need
        
        
            location.GetComponent<AnimationManager>().PlayAnimationHere("skip");
        
    }
    void CallFunctionOnLocationPlus2(GameObject location)
    {
        // Example function call - replace this with what you need


        location.GetComponent<AnimationManager>().PlayAnimationHere("plus2");

    }
    void CallFunctionOnLocationPlus4(GameObject location)
    {
        // Example function call - replace this with what you need


        location.GetComponent<AnimationManager>().PlayAnimationHere("plus4");

    }

    // Skip the next player and make them draw the specified number of cards
    private void SkipAndDrawCards(int numberOfCards)
    {
        UpdateTurn(); // Move to the next player
        List<(int, int)> currentPlayerHand = GetCurrentPlayerHand();

        // Draw the specified number of cards
        for (int i = 0; i < numberOfCards; i++)
        {
            (int, int) drawnCard = deckManager.DrawCardFromDeck();
            if (drawnCard != (-1, -1))
            {
                if (numberOfCards == 2)
                {
                    imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromDeckToHand(handVectors[turnIndex], 0.5f, imageTransform);
                    imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromDeckToHand(handVectors[turnIndex], 0.6f, imageTransform2);
                }
                if (numberOfCards == 4)
                {
                    imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromDeckToHand(handVectors[turnIndex], 0.5f, imageTransform);
                    imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromDeckToHand(handVectors[turnIndex], 0.6f, imageTransform2);
                    imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromDeckToHand(handVectors[turnIndex], 0.7f, imageTransform3);
                    imageMoverManager.GetComponent<MoveUIImage>().MoveCardFromDeckToHand(handVectors[turnIndex], 0.8f, imageTransform4);
                }


                currentPlayerHand.Add(drawnCard);
                if (turnIndex == 0)
                {
                    AddCardToPlayerHandUI(drawnCard);
                }
                Debug.Log($"Player at index {turnIndex} drew a card: ({drawnCard.Item1}, {drawnCard.Item2}).");
                
                Debug.Log(FormatHand(playerHand));
                Debug.Log(FormatHand(aiHand1));
                Debug.Log(FormatHand(aiHand2));
                Debug.Log(FormatHand(aiHand3));
            }
            else
            {
                Debug.Log("No more cards in the deck to draw!");
            }
        }
        if (numberOfCards == 2)
        {
            CallFunctionOnLocationPlus2(animationLocations[turnIndex + 4]);
        }
        if (numberOfCards == 4)
        {
            CallFunctionOnLocationPlus4(animationLocations[turnIndex + 8]);
        }

        CallFunctionOnLocationSkip(animationLocations[turnIndex]);
        Debug.Log($"Player at index {turnIndex} has drawn {numberOfCards} cards and will be skipped.");
        // Skip this player's turn
    }

    // Returns the current player's hand based on the turnIndex
    private List<(int, int)> GetCurrentPlayerHand()
    {
        switch (turnIndex)
        {
            case 0:
                return playerHand;
            case 1:
                return aiHand1;
            case 2:
                return aiHand2;
            case 3:
                return aiHand3;
            default:
                Debug.LogError("Invalid player index");
                return null;
        }
    }




    private string FormatHand(List<(int, int)> hand)
    {
        string handString = "";
        foreach (var card in hand)
        {
            handString += $"({card.Item1}, {card.Item2}) "; // Add each card as a tuple
        }
        return handString;
    }


    public void InsaneMode()
    {
        insaneModeOn = true;
        Debug.Log("insane mode activated");
        for (int i = 0; i < 7; i++)
        {
            (int, int) drawnCard = deckManager.DrawCardFromDeck();
            playerHand.Add(drawnCard);
            AddCardToPlayerHandUI(drawnCard);


        }

        name1GUI.text = "???";
        name2GUI.text = "???";
        name3GUI.text = "???";

        aiHand1.RemoveRange(3, aiHand1.Count - 3);

        aiHand2.RemoveRange(3, aiHand2.Count - 3);

        aiHand3.RemoveRange(3, aiHand3.Count - 3);

        UpdateHandCounts();

        LoadBackgroundSprite("BackgroundInsane");

    }
    private void LoadBackgroundSprite(string spriteName)
    {
        // Load the sprite from the Resources folder
        Sprite loadedSprite = Resources.Load<Sprite>(spriteName);

        if (loadedSprite != null)
        {
            // Get the SpriteRenderer component from the background GameObject
            SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = loadedSprite; // Assign the loaded sprite
            }
            else
            {
                Debug.LogError("SpriteRenderer not found on background GameObject.");
            }
        }
        else
        {
            Debug.LogError($"Failed to load sprite: {spriteName}. Make sure it is in the Resources folder.");
        }
    }

    public void ImpossibleModecaller()
    {
        StartCoroutine(ImpossibleMode());
    }
    public IEnumerator ImpossibleMode()
    {
        RedImage.SetActive(true);
        RedImageText.text = "";
        yield return new WaitForSeconds(2f);
        RedImageText.text = "You were told you wouldn't find a challenge here and yet you came?";
        yield return new WaitForSeconds(4f);
        RedImageText.text = "";
        yield return new WaitForSeconds(2f);
        RedImageText.text = "Fine then";
        yield return new WaitForSeconds(2f);
        RedImageText.text = "";
        yield return new WaitForSeconds(1f);
        RedImageText.text = "You are not getting out of this";
        yield return new WaitForSeconds(3f);



        impossibleModeOn = true;
        Debug.Log("impossible mode activated");
        for (int i = 0; i < 40; i++)
        {
            (int, int) drawnCard = deckManager.DrawCardFromDeck();
            playerHand.Add(drawnCard);
            AddCardToPlayerHandUI(drawnCard);


        }

        name1GUI.text = "???";
        name2GUI.text = "???";
        name3GUI.text = "???";

        
        
        aiHand1.RemoveRange(1, aiHand1.Count-1); 

        aiHand2.RemoveRange(1, aiHand2.Count - 1);

        aiHand3.RemoveRange(1, aiHand3.Count - 1);

        UpdateHandCounts();

        LoadBackgroundSprite("BackgroundImpossible");


        RedImage.SetActive (false);
    }

}