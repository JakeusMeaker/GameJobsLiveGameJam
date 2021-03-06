﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    private void Awake()
    {
        instance = this;
    }

    AudioManager audioManager;

    public Sprite fullHealthSprite;
    public Sprite emptyHealthSprite;
    public Sprite fullStaminaSprite;
    public Sprite emptyStaminaSprite;

    [Header("Setup Variables")]
    public GameObject characterSelectCanvas;
    public GameObject mainCharacterCanvas;

    [Header("MainCanvas Variables")]
    public Text[] traitButtons;
    public Text[] characterNames;

    [Header("Game Scene Setup")]
    public Sprite[] SideFacingCharacterSprites;
    public Sprite[] FrontFacingCharacterSprites;
    public SpriteRenderer character1Sprite;
    public SpriteRenderer character2Sprite;
    public SpriteRenderer character3Sprite;
    public CharacterUIData[] characterUI = new CharacterUIData[3];


    public SO_Character[] characterList;
    public List<SO_Character> selectedCharacters = new List<SO_Character>();
    public List<E_Trait> selectedTraits = new List<E_Trait>(6);
    private void Start()
    {
        for (int i = 0; i < characterList.Length; i++)
        {
            characterList[i].InitCharacter();
        }

        CharacterSelectionManager.instance.PopulateUICharacters();
        audioManager = AudioManager.instance;
    }

    public void CharacterSelectButton(int buttonPress)
    {
        selectedCharacters.Add(characterList[buttonPress]);

        selectedTraits.Add(characterList[buttonPress].firstTrait);
        selectedTraits.Add(characterList[buttonPress].secondTrait);

        Debug.Log(characterList[buttonPress].characterName);

        if (selectedCharacters.Count == 3)
        {
            Debug.Log("CHARACTERS SELECTED");
            characterUI[0].thisCharacter = selectedCharacters[0];
            characterUI[1].thisCharacter = selectedCharacters[1];
            characterUI[2].thisCharacter = selectedCharacters[2];


            StartCoroutine(IE_GameTransition());
            // Move Forwards
        }
    }

    IEnumerator IE_GameTransition()
    {
        BlackoutAnimator.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        TransitionToGame();
        BlackoutAnimator.instance.FadeFromBlack();
    }


    void TransitionToGame()
    {
        characterSelectCanvas.SetActive(false);
        mainCharacterCanvas.SetActive(true);

        for (int i = 0; i < traitButtons.Length; i++)
        {
            traitButtons[i].text = selectedTraits[i].ToString();
        }
        for (int i = 0; i < selectedCharacters.Count; i++)
        {
            characterNames[i].text = selectedCharacters[i].characterName;
        }

        character1Sprite.sprite = SideFacingCharacterSprites[Random.Range(0, SideFacingCharacterSprites.Length)];
        character2Sprite.sprite = FrontFacingCharacterSprites[Random.Range(0, FrontFacingCharacterSprites.Length)];
        character3Sprite.sprite = SideFacingCharacterSprites[Random.Range(0, SideFacingCharacterSprites.Length)];
        
        ScenarioManager.AStartScenario();
    }

    //THIS FUNCTION IS WHERE WE CHOOSE THE SELECTED TRAIT TO USE
    public void OnTraitButtonPress(int buttonPosition)
    {
        int _charNumber = (int)buttonPosition / 2; //This is the Character that used this. Can use this to decrement health/stamina.

        if (selectedCharacters[_charNumber].isDead == true)
            return;

        string debugLog = selectedTraits[buttonPosition].ToString() + " was used by " + selectedCharacters[_charNumber].characterName;
        ScenarioManager.ATraitSelected(selectedTraits[buttonPosition], selectedCharacters[_charNumber]);
        Debug.Log(debugLog);
    }

    public void AdjustStamina(SO_Character _char, int adjust)
    {
        if (_char.isDead)
            return;

        if (_char.stamina == 0 && adjust < 0)
        {
            AdjustHealth(_char, -1);
        }

        _char.stamina += adjust;
        _char.stamina = Mathf.Clamp(_char.stamina, 0, 2);

        HandleStaminaUI(_char, _char.stamina);
    }


    public void AdjustHealth(SO_Character _char, int adjust)
    {
        if (_char.isDead)
            return;

        _char.health += adjust;
        _char.health = Mathf.Clamp(_char.health, 0, 3);
        if (adjust < 0)
        {
            CameraShake.instance.ShakeCamera(.5f, .5f);
            audioManager.Play(E_SFX.Injury);

            for (int i = 0; i < characterUI.Length; i++)
            {
                if (_char == characterUI[i].thisCharacter)
                {
                    if (_char.health > 0)
                    {
                        switch (i)
                        {
                            case 0:
                                StartCoroutine(InjuryLerp(character1Sprite, 1f));
                                break;
                            case 1:
                                StartCoroutine(InjuryLerp(character2Sprite, 1f));
                                break;
                            case 2:
                                StartCoroutine(InjuryLerp(character3Sprite, 1f));
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        KillCharacter(_char);
                    }
                }
            }
        }

        HandleHealthUI(_char, _char.health);
    }
    void HandleStaminaUI(SO_Character character, int stamina)
    {
        int charIndex = 0;
        Mathf.Clamp(stamina, 0, 2);
        for (int i = 0; i < characterUI.Length; i++)
        {
            if (character == characterUI[i].thisCharacter)
            {
                charIndex = i;
            }
        }
        switch (stamina)
        {
            case 0:
                characterUI[charIndex].stamina[0].sprite = emptyStaminaSprite;
                characterUI[charIndex].stamina[1].sprite = emptyStaminaSprite;
                break;
            case 1:
                characterUI[charIndex].stamina[0].sprite = fullStaminaSprite;
                characterUI[charIndex].stamina[1].sprite = emptyStaminaSprite;
                break;
            case 2:
                characterUI[charIndex].stamina[0].sprite = fullStaminaSprite;
                characterUI[charIndex].stamina[1].sprite = fullStaminaSprite;
                break;
            default:
                break;
        }
    }

    void HandleHealthUI(SO_Character character, int health)
    {
        int charIndex = 0;
        Mathf.Clamp(health, 0, 3);
        for (int i = 0; i < characterUI.Length; i++)
        {
            if (character == characterUI[i].thisCharacter)
            {
                charIndex = i;
            }
        }
        switch (health)
        {
            case 0:
                characterUI[charIndex].health[0].sprite = emptyHealthSprite;
                characterUI[charIndex].health[1].sprite = emptyHealthSprite;
                characterUI[charIndex].health[2].sprite = emptyHealthSprite;
                break;
            case 1:
                characterUI[charIndex].health[0].sprite = fullHealthSprite;
                characterUI[charIndex].health[1].sprite = emptyHealthSprite;
                characterUI[charIndex].health[2].sprite = emptyHealthSprite;
                break;
            case 2:
                characterUI[charIndex].health[0].sprite = fullHealthSprite;
                characterUI[charIndex].health[1].sprite = fullHealthSprite;
                characterUI[charIndex].health[2].sprite = emptyHealthSprite;
                break;
            case 3:
                characterUI[charIndex].health[0].sprite = fullHealthSprite;
                characterUI[charIndex].health[1].sprite = fullHealthSprite;
                characterUI[charIndex].health[2].sprite = fullHealthSprite;
                break;
            default:
                break;
        }
    }

    public void CharacterSelectAudio()
    {
        audioManager.Play(E_SFX.Heal);
    }

    public void KillCharacter(SO_Character character)
    {
        Debug.Log("Killed " + character.characterName);
        for (int i = 0; i < characterUI.Length; i++)
        {
            if (character == characterUI[i].thisCharacter)
            {
                selectedCharacters[i].isDead = true;

                switch (i)
                {
                    case 0:
                        StartCoroutine(DeathLerp(character1Sprite, 1f));
                        break;
                    case 1:
                        StartCoroutine(DeathLerp(character2Sprite, 1f));
                        break;
                    case 2:
                        StartCoroutine(DeathLerp(character3Sprite, 1f));
                        break;
                    default:
                        break;
                }
            }
        }
    }

    IEnumerator DeathLerp(SpriteRenderer valueToChange, float duration)
    {
        float time = 0;
        Color startValue = valueToChange.color;
        Color endValue = Color.clear;

        while (time < duration)
        {
            //Debug.Log("Lerping");
            valueToChange.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        valueToChange.color = endValue;
    }

    IEnumerator InjuryLerp(SpriteRenderer valueToChange, float duration)
    {
        float time = 0;
        Color startValue = Color.red;
        Color endValue = Color.white;

        while (time < duration)
        {
            //Debug.Log("Lerping");
            valueToChange.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        valueToChange.color = endValue;
    }
}
