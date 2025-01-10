using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{
    public GameObject WinningGui;
    public TextMeshProUGUI winnerNameText;
    string winningText;
    public GameObject BlackImage;
    public TextMeshProUGUI BlackImageText;
    public GameObject WhiteImage;
    public TextMeshProUGUI WhiteImageText;
    public GameObject restartButton;
    // Start is called before the first frame update
    void Start()
    {
        WinningGui.SetActive(false);
        BlackImage.SetActive(false);
        restartButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Winner(string winnerName)
    {
       if (winnerName == "You")
       {
           winnerNameText.text = "You win";
       }
       else
       {
           winningText = $"{winnerName} wins";
           winnerNameText.text = winningText;
       }
       WinningGui.SetActive(true);
        
    }
    public void WinnerInsanePlayer()
    {
        StartCoroutine(YouWonInsane());
    }
    public void WinnerInsaneAi()
    {
        StartCoroutine(YouDied());
    }
    private IEnumerator YouDied()
    {
        BlackImage.SetActive(true);
        BlackImageText.text = "";
        yield return new WaitForSeconds(2f);
        BlackImageText.text = "You tried your best";
        yield return new WaitForSeconds(2f);
        BlackImageText.text = "";
        yield return new WaitForSeconds(2f);
        BlackImageText.text = "It was not good enough";
        yield return new WaitForSeconds(2f);
        BlackImageText.text = "";
        yield return new WaitForSeconds(1f);
        BlackImageText.text = ".";
        yield return new WaitForSeconds(1f);
        BlackImageText.text = "..";
        yield return new WaitForSeconds(1f);
        BlackImageText.text = "...";
        yield return new WaitForSeconds(3f);
        BlackImageText.text = "YOU LOST";

        StopAllCoroutines();
    }
    private IEnumerator YouWonInsane()
    {
        WhiteImage.SetActive(true);
        WhiteImageText.text = "";
        yield return new WaitForSeconds(2f);
        WhiteImageText.text = "Well done";
        yield return new WaitForSeconds(2f);
        WhiteImageText.text = "";
        yield return new WaitForSeconds(2f);
        WhiteImageText.text = "You truly are the god of NNUno";
        yield return new WaitForSeconds(3f);
        WhiteImageText.text = "";
        yield return new WaitForSeconds(2f);
        WhiteImageText.text = "A man without equal";
        yield return new WaitForSeconds(2f);
        WhiteImageText.text = "";
        yield return new WaitForSeconds(2f);
        WhiteImageText.text = "Go on";
        yield return new WaitForSeconds(2f);
        WhiteImageText.text = "";
        yield return new WaitForSeconds(2f);
        WhiteImageText.text = "Play again if you wish to do so";
        yield return new WaitForSeconds(3f);
        WhiteImageText.text = "";
        yield return new WaitForSeconds(2f);
        WhiteImageText.text = "I doubt that a master like you...";
        yield return new WaitForSeconds(3f);
        WhiteImageText.text = "";
        yield return new WaitForSeconds(2f);
        WhiteImageText.text = "Would find a challenge in the mode I call..";
        yield return new WaitForSeconds(2f);
        WhiteImageText.text = "";
        yield return new WaitForSeconds(2f);
        WhiteImageText.text = "IMPOSSIBLE";
        yield return new WaitForSeconds(2f);
        restartButton.SetActive(true);



        StopAllCoroutines();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
