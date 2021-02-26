using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character", order = 0)]
public class SO_Character : ScriptableObject
{
    public string characterName;

    public E_Trait firstTrait;
    public E_Trait secondTrait;

    public int stamina = 3;
    public int health = 3;

   public bool isDead = false;

    public void InitCharacter()
    {
        GenerateName();
        GenerateTraits();
        health = 3;
        stamina = 3;

        isDead = false;

        //Debug.Log(characterName + ", " + firstTrait + ", " + secondTrait);
    }

    void GenerateName()
    {
        int _rFN = System.Enum.GetValues(typeof(E_FirstName)).Length;
        E_FirstName _fn = (E_FirstName)Random.Range(0, _rFN);

        int _rLN = System.Enum.GetValues(typeof(E_LastName)).Length;
        E_LastName _ln = (E_LastName)Random.Range(0, _rLN);

        //characterName = _fn.ToString();        
        characterName = _fn.ToString() + " " + _ln.ToString();
    }

    void GenerateTraits()
    {
        int _random = System.Enum.GetValues(typeof(E_Trait)).Length;
        firstTrait = (E_Trait)Random.Range(0, _random);
        secondTrait = (E_Trait)Random.Range(0, _random);
        while (firstTrait == secondTrait)
        {
            secondTrait = (E_Trait)Random.Range(0, _random);
        }
    }
}
