using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
#region Singleton
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            //Debug.LogError("Multiple AudioManagers");
            Destroy(this.gameObject);
        }
    }
#endregion

    public AudioSource SFXSource;

    public AudioClip[] typewriter;

    public AudioClip[] injury;
    public AudioClip death;
    public AudioClip success;
    public AudioClip heal;

    void PlayAudio(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void Play(E_SFX clip)
    {
        switch (clip)
        {
            case E_SFX.Injury:
                PlayAudio(injury[Random.Range(0, injury.Length)]);
                break;
            case E_SFX.Success:
                PlayAudio(success);
                break;
            case E_SFX.Heal:
                PlayAudio(heal);
                break;
            case E_SFX.Death:
                PlayAudio(death);
                break;
            case E_SFX.GameOver:
                PlayAudio(death);
                break;
            default:
                break;
        }
    }

    public void Typewriter()
    {
        PlayAudio(typewriter[Random.Range(0, typewriter.Length)]);
    }
}
