using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameStartSetup : MonoBehaviour
{
    public GameObject blackoutScreen;
    public GameObject characterSelectCanvas;
    public GameObject gameScreenCanvas;

    private void Awake()
    {
        blackoutScreen.SetActive(true);
        characterSelectCanvas.SetActive(true);
        gameScreenCanvas.SetActive(false);
    }
}
