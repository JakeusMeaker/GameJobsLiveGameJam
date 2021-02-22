using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    #region Singleton
    public static CharacterSelectionManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("Multiple CharacterSelectionManagers in Scene");
            Destroy(this.gameObject);
        }
    }
    #endregion

    public SO_Character[] characters;

    public Text[] characterNames;
    public Text[] firstTraits;
    public Text[] secondTraits;

    [Header ("Selected Characters")]
    public Text[] selectionNames;
    public Text[] selectionTrait1;
    public Text[] selectionTrait2;

    public void PopulateUICharacters()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            characterNames[i].text = characters[i].characterName;
            firstTraits[i].text = characters[i].firstTrait.ToString();
            secondTraits[i].text = characters[i].secondTrait.ToString();
        }
    }

}
