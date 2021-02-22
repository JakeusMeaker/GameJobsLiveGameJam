using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [Header("Setup Variables")]
    public GameObject characterSelectCanvas;
    public GameObject mainCharacterCanvas;

    [Header("MainCanvas Variables")]
    public Text[] traitButtons;
    public Text[] characterNames;

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
            TransitionToGame();
            // Move Forwards
        }
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
    }

    //THIS FUNCTION IS WHERE WE CHOOSE THE SELECTED TRAIT TO USE
    public void OnTraitButtonPress(int buttonPosition)
    {
        int _charNumber = (int)buttonPosition / 2; //This is the Character that used this. Can use this to decrement health/stamina.

        string debugLog = selectedTraits[buttonPosition].ToString() + " was used by " + selectedCharacters[_charNumber].characterName;

        Debug.Log(debugLog);
    }
}
