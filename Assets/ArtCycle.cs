using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtCycle : MonoBehaviour
{
    Image thisImage;

    public Sprite[] availableSprites;

    private void Start()
    {
        thisImage = GetComponent<Image>();
    }

    void ChooseNewSprite()
    {
        Sprite _temp = availableSprites[Random.Range(0, availableSprites.Length)];

        thisImage.sprite = _temp;
    }
}
