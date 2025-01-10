using TMPro; // Add this to work with TextMeshPro
using UnityEngine;

public class TextInputManager : MonoBehaviour
{
    public GameObject inputFieldObject;
    public TMP_InputField inputField; // Reference to the TMP_InputField UI component
    private string playerName;
    public TextMeshProUGUI playerNameGUI;
    public GameObject cardGameManagerObj;
    public GameObject winManager;

    // Start is called before the first frame update
    void Start()
    {
        // Add a listener to the input field to call SaveInputOnEnter when editing is finished
        inputField.onEndEdit.AddListener(SaveInputOnEnter);
        inputFieldObject.SetActive(true);
        
    }

    public void SaveInputOnEnter(string inputText)
    {
        // Check if Enter (Submit) was pressed
        if (Input.GetKeyDown(KeyCode.Return)) // or KeyCode.KeypadEnter for the numpad enter
        {
            playerName = inputText; // Save the input in the string variable
            Debug.Log("Name saved: " + playerName);
            playerNameGUI.text = playerName; // Update the text UI with the player's name
            if (playerName == "Insane" || playerName == "insane")
            {
                Debug.Log("yyyyyyyyyyyyyyyyyyyyyyyyyyyy");
                cardGameManagerObj.GetComponent<CardGameManager>().InsaneMode();
            }
            if (playerName == "insaneinstawin")
            {
                winManager.GetComponent<WinScript>().WinnerInsanePlayer();
            }
            if (playerName == "Impossible" || playerName == "impossible")
            {
                cardGameManagerObj.GetComponent<CardGameManager>().ImpossibleModecaller();
            }

            inputFieldObject.SetActive(false);

        }
    }
}
