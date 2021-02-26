using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public AudioMixer masterMixer;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OptionsMenu()
    {
        anim.SetBool("Options", !anim.GetBool("Options"));
    }

    public void StartButton()
    {
        anim.SetBool("StartGame", true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SetMasterVolume(float volume)
    {
        masterMixer.SetFloat("MasterVolume", volume);
    }

    public void SetBGMVolume(float volume)
    {
        masterMixer.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat("SFXVolume", volume);
    }

    public void CheckSFXVolume()
    {
        AudioManager.instance.Play(E_SFX.Injury);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
