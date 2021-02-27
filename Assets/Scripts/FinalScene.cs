using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class FinalScene : MonoBehaviour
{
    public GameObject[] thingsToDisable;

    public GameObject finalScene;

    public Sprite[] gameWinSprite;
    public Sprite gameOverSprite;

    public Text epilogueText;
    public SpriteRenderer backgroundImage;
    public GameObject[] playerSprites;

    private void Start()
    {
        finalScene.SetActive(false);
        for (int i = 0; i < playerSprites.Length; i++)
        {
            playerSprites[i].SetActive(false);
        }
    }
    //Some fancy spiel 

    public void DoFinalScene(SO_Character[] characters, bool gameWin)
    {
        StartCoroutine(IE_EndGame(characters, gameWin));
    }

    IEnumerator IE_EndGame(SO_Character[] characters, bool gameWin)
    {
        BlackoutAnimator.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < thingsToDisable.Length; i++)
        {
            thingsToDisable[i].SetActive(false);
        }
        FinalSceneCode(characters, gameWin);
        BlackoutAnimator.instance.FadeFromBlack();
    }

    void FinalSceneCode(SO_Character[] characters, bool gameWin)
    {
        if (gameWin == true)
        {
            int _winsprite = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i].isDead == false)
                    _winsprite++;
            }
            backgroundImage.sprite = gameWinSprite[_winsprite - 1];
        }
        else
            backgroundImage.sprite = gameOverSprite;


        for (int i = 0; i < characters.Length; i++)
        {
            epilogueText.text += "\n" + characters[i].characterName + EpilogueText(characters[i].isDead, Random.Range(0, 11)) + "\n";
            if (characters[i].isDead == false)
            {
                playerSprites[i].SetActive(true);
            }
        }
    }

    public void ResetGame()
    {
        Destroy(AudioManager.instance.gameObject);
        SceneManager.LoadScene(0);
    }

    string EpilogueText(bool _isDead, int _choice)
    {
        string _text = "";
        if (_isDead == true)
        {
            switch (_choice)
            {
                case 0:
                    _text += " was lost, never to be seen again.";
                    break;
                case 1:
                    _text += "'s parents divorced later, the loss was too great.";
                    break;
                case 2:
                    _text += " will be missed.";
                    break;
                case 3:
                    _text += " leaves behind a grieving partner and a puppy.";
                    break;
                case 4:
                    _text += " never got a chance to realise their potential.";
                    break;
                case 5:
                    _text += " became an urban legend, the name of the Ghost that haunts the house";
                    break;
                case 6:
                    _text += " remains missing to this day.";
                    break;
                case 7:
                    _text += " is the subject of a massive manhunt, and is never found.";
                    break;
                case 8:
                    _text += " was never noticed to be gone, outside of those that dared them to enter.";
                    break;
                case 9:
                    _text += "'s disappearence caused an uproar which resulted in the demolition of the House.";
                    break;
                case 10:
                    _text += " was never seen again, but never forgotten.";
                    break;
                case 11:
                    _text += "'s body was never discovered. It was a closed casket ceremony.";
                    break;
  
                default:
                    break;
            }
        }
        else
        {
            switch (_choice)
            {
                case 0:
                    _text += " escaped, soon counting everything they saw as a bad dream and settling back into a mostly normal life.";
                    break;
                case 1:
                    _text += " managed to flee, but was sectioned for their outlandish tales and spends their days in an observed, medicated stupor.";
                    break;
                case 2:
                    _text += " got out alive, but was arrested shortly afterwards for burning the House down. They are serving time for Arson.";
                    break;
                case 3:
                    _text += " escaped and, after deep councelling, came to terms with what they saw inside.";
                    break;
                case 4:
                    _text += " wrote a book about their experiences, and despite marketing it as a true story, topped the Horror charts.";
                    break;
                case 5:
                    _text += " moved out of town shortly afterwards. They deny all knowledge of it if asked.";
                    break;
                case 6:
                    _text += " turned to habitual drug use shortly afterwards, mumbling about ghosts and demons, and being unclean. They are getting the help they need.";
                    break;
                case 7:
                    _text += " got out, leading the police back to the house afterwards in an effort to find others. They found nothing.";
                    break;
                case 8:
                    _text += " escaped intact, but still wakes up in a cold sweat thinking about it.";
                    break;
                case 9:
                    _text += " got out and, with the help of counselling, eventually left the House behind them.";
                    break;
                case 10:
                    _text += " collapsed shortly after leaving the house. When they awoke in hospital, weeks later, they mercifully remembered nothing.";
                    break;
                case 11:
                    _text += " became quiet and withdrawn after they got out. Shortly afterwards, the House burned down, and they vanished.";
                    break;
                default:
                    break;
            }
        }
        return _text;
    }
}
