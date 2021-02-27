using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Vector2 timeBetweenFlashes;
    float timer;
    float delay;

    Animator anim;
    public Animator lightningAnimator;
    public AudioSource lightningAudio;

    private void Start()
    {
        anim = GetComponent<Animator>();
        SetFullscreen(false);

        SetLightningDelay();
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            timer = 0;
            LightningCrash();
            SetLightningDelay();
        }
    }

    void SetLightningDelay()
    {
        delay = Random.Range(timeBetweenFlashes.x, timeBetweenFlashes.y);

    }

    void LightningCrash()
    {
        lightningAnimator.SetTrigger("Thunder");
        lightningAudio.Play();
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
        if (isFullscreen == true)
        {
            Screen.SetResolution(960, 720, true);
        }
        else
        {
            Screen.SetResolution(960, 720, false);
        }
    }
}
