using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] voiceOvers;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = voiceOvers[Random.Range(0, voiceOvers.Length)];
        audioSource.Play();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void SkipButton()
    {
        GetComponent<Animator>().SetTrigger("Start");

    }
}
