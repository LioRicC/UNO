using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    Button redButton;
    Button greenButton;
    Button blueButton;
    Button yellowButton;
    public (int, int) chosenCol;
    public GameObject colorGui;
    public bool playerCanChoose;
    public bool playerHasChosenCol;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateColorGUI()
    {
        colorGui.SetActive(true);

        playerHasChosenCol = false;


    }

    

    public void RedButtonPressed()
    {
        chosenCol = (14, 1);
        playerHasChosenCol = true;
    }
    public void GreenButtonPressed()
    {
        chosenCol = (14, 2);
        playerHasChosenCol = true;
    }
    public void BlueButtonPressed()
    {
        chosenCol = (14, 3);
        playerHasChosenCol = true;
    }
    public void YellowButtonPressed()
    {
        chosenCol = (14, 4);
        playerHasChosenCol = true;
    }
}
