using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public SO_Character[] selectedCharacters = new SO_Character[3];

    private void Start()
    {
        for (int i = 0; i < selectedCharacters.Length; i++)
        {
            selectedCharacters[i].InitCharacter();
        }

        CharacterSelectionManager.instance.PopulateUICharacters();
    }
}
