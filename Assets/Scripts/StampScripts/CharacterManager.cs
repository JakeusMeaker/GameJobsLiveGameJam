using System.Collections;
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

    public Animator charSpriteAnim;

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
        yield return new WaitForSeconds (1.5f);
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

        string debugLog = selectedTraits[buttonPosition].ToString() + " was used by " + selectedCharacters[_charNumber].characterName;
        ScenarioManager.ATraitSelected(selectedTraits[buttonPosition], selectedCharacters[_charNumber]);
        Debug.Log(debugLog);
    }

    public void AdjustStamina(SO_Character _char, int adjust)
    {
        _char.stamina += adjust;
        for (int i = 0; i < characterUI.Length; i++)
        {
            if (_char == characterUI[i].thisCharacter)
            {
                characterUI[i].stamina[_char.stamina].sprite = emptyStaminaSprite;
            }
        }
    }

    public void AdjustHealth(SO_Character _char, int adjust)
    {
        _char.health += adjust;
        if (adjust < 0)
        {
            CameraShake.instance.ShakeCamera(.5f, .5f);
        }

        for (int i = 0; i < characterUI.Length; i++)
        {
            if (_char == characterUI[i].thisCharacter)
            {
                characterUI[i].health[_char.health].sprite = emptyHealthSprite;
                charSpriteAnim.SetInteger("CharacterDamaged", i);
                charSpriteAnim.SetTrigger("Damage");
            }
        }
    }
}
