using UnityEngine;

public class CardAssets : MonoBehaviour
{
    public static CardAssets Instance { get; private set; }

    public Sprite[] ColorSprites;  // Array of sprites for card colors
    public Sprite[] NumberSprites; // Array of sprites for card numbers

    private void Awake()
    {
        Instance = this;
    }
}

