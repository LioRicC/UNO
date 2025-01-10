using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoveUIImage : MonoBehaviour
{
    //public RectTransform imageTransform;  // UI Image's RectTransform
    public Vector2 deckPosition = new Vector2(65.88f, -33f);         // Starting position (x, y)
    public Vector2 fieldPosition = new Vector2(-116.7948f, -33f);          // Target position (x, y)
    public float duration = 0.5f;           // Duration of the movement

    private void Start()
    {
        // Start the movement when the game starts
        


    }

    public void MoveCardFromDeckToHand(Vector2 endPosition,float individualDuration, RectTransform imageTransform)
    {

        StartCoroutine(MoveOverTime(deckPosition, endPosition, individualDuration, imageTransform));
    }
    public void MoveCardFromHandToField(Vector2 startPosition, float individualDuration, RectTransform imageTransform)
    {

        StartCoroutine(MoveOverTime(startPosition, fieldPosition, individualDuration, imageTransform));
    }

    IEnumerator MoveOverTime(Vector2 from, Vector2 to, float time,RectTransform imageTransform)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            // Lerp between the start and end positions for 2D (Vector2)
            imageTransform.anchoredPosition = Vector2.Lerp(from, to, elapsedTime / time);

            // Increase the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Ensure the image is exactly at the end position
        imageTransform.anchoredPosition = to;
    }
}