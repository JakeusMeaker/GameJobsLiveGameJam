using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioManager : MonoBehaviour
{
    public SpriteRenderer backgroundSprite;
    public SpriteRenderer foregroundSprite;

    [Header("Scenario Generation"), Range(3, 9)]
    public int amountOfScenarios;

    [Range(0.5f, 10f)]
    public float scenarioChangeTime;

    public int healthGainInRestRoomAmount;
    public int staminaGainInRestRoomAmount;

    [SerializeField]
    private List<ScenarioSO> scenarioList = new List<ScenarioSO>();

    private Queue<ScenarioSO> scenarioQueue = new Queue<ScenarioSO>();
    private ScenarioSO currentScenario;
    private E_Trait lastTraitUsed;
    private bool changeScenario = false;

    [Header("Scenario UI Elements")]
    public Text scenarioTextBox;
    public GameObject restRoomContinueButton;

    private int partyMembersDown = 0;

    public static Action<E_Trait, SO_Character> ATraitSelected;
    public static Action AStartScenario;

    [Header("Trait Percentages"), Range(0f, 1f)]
    public float secondaryTraitSuccessPercent;
    private E_PassType passType;

    private CharacterManager characterManager = null;
    private bool canUseTrait = true;

    // Start is called before the first frame update
    void Start()
    {
        characterManager = GetComponent<CharacterManager>();
        ATraitSelected += TraitSelected;
        AStartScenario += StartCurrentScenario;
        GenerateScenarioQueue();
        currentScenario = scenarioQueue.Peek();
        LoadScenarioSprites();
    }

    private void GenerateScenarioQueue()
    {
        while (scenarioQueue.Count < amountOfScenarios)
        {
            if (scenarioList.Count == 1)
            {
                scenarioQueue.Enqueue(scenarioList[0]);
                return;
            }

            ScenarioSO scenario = scenarioList[UnityEngine.Random.Range(0, scenarioList.Count)];
            if (scenarioQueue != null)
            {
                if (scenario.traitToPass != lastTraitUsed)
                {
                    scenarioQueue.Enqueue(scenario);
                    lastTraitUsed = scenario.traitToPass;
                }
            }
            else
            {
                scenarioQueue.Enqueue(scenario);
                lastTraitUsed = scenario.traitToPass;
            }
        }
    }

    public void NextScenario()
    {
        StartCoroutine(IE_LoadNextScene());
    }

    IEnumerator IE_LoadNextScene()
    {
        BlackoutAnimator.instance.FadeToBlack();
        yield return new WaitForSecondsRealtime(1.5f);
        scenarioTextBox.text = "";
        LoadNextScenario();
        BlackoutAnimator.instance.FadeFromBlack();
        canUseTrait = true;
    }

    public void LoadNextScenario()
    {
        if (scenarioQueue.Count > 0)
        {
            scenarioQueue.Dequeue();
            currentScenario = scenarioQueue.Peek();
            if (currentScenario.isRestRoom)
            {
                restRoomContinueButton.SetActive(true);
                canUseTrait = false;

                //Probably a more elegant way of doing this
                for (int i = 0; i < characterManager.selectedCharacters.Count; i++)
                {
                    if (characterManager.selectedCharacters[i].health < 3)
                        characterManager.AdjustHealth(characterManager.selectedCharacters[i], healthGainInRestRoomAmount);
                    if (characterManager.selectedCharacters[i].stamina < 3)
                        characterManager.AdjustStamina(characterManager.selectedCharacters[i], staminaGainInRestRoomAmount);
                }
            }
            else
            {
                restRoomContinueButton.SetActive(false);
                canUseTrait = true;
            }
            LoadScenarioSprites();
            Invoke("StartCurrentScenario", scenarioChangeTime);
        }
        else
        {
            //End game condititions here 

        }
      }

    void LoadScenarioSprites()
    {
        if (currentScenario.backgroundImage != null)
            backgroundSprite.sprite = currentScenario.backgroundImage;
        else
            backgroundSprite.sprite = null;
        if (currentScenario.foregroundImage != null)
            foregroundSprite.sprite = currentScenario.foregroundImage;
        else
            foregroundSprite.sprite = null;
    }

    public void StartCurrentScenario()
    {
        StartCoroutine(TextTyper(currentScenario.scenarioText));
    }

    IEnumerator TextTyper(string textToType)
    {
        scenarioTextBox.text = "";
        for (int i = 0; i < textToType.Length; i++)
        {
            scenarioTextBox.text += textToType[i];
            yield return new WaitForSeconds(0.05f);
        }

        if (changeScenario)
        {
            changeScenario = false;
            Invoke("NextScenario", scenarioChangeTime);
        }
        else        
            yield return null;
    }

    public void TraitSelected(E_Trait trait, SO_Character character)
    {
        /* Rewrite so that:
         * - The main trait is guarenteed success - Done
         * - The Secondary traits get individual win text - Done
         * - Secondary's have percentage to fail - Done
         * - Lucky trait always has 50% chance of winning - Done
         * - Lucky has generic lucky win text - Done
         * - Fail conditions are checked and accurately respond to the players condition - Done
         * 
         *  Will review when less sleepy
         */

        if (canUseTrait)
        {
            canUseTrait = false;
            if (trait == scenarioQueue.Peek().traitToPass)
            {
                passType = E_PassType.TraitPass;
            }
            else if (trait == scenarioQueue.Peek().secondaryTraits[0])
            {
                if (UnityEngine.Random.value > secondaryTraitSuccessPercent)
                {
                    passType = E_PassType.Secondary1Pass;
                }
                else
                {
                    passType = E_PassType.Fail;
                }
            }
            else if (trait == scenarioQueue.Peek().secondaryTraits[1])
            {
                if (UnityEngine.Random.value > secondaryTraitSuccessPercent)
                {
                    passType = E_PassType.Secondary2Pass;
                }
                else
                {
                    passType = E_PassType.Fail;
                }
            }
            else if (trait == E_Trait.Lucky)
            {
                if (UnityEngine.Random.value > 0.5f)
                {
                    passType = E_PassType.LuckyPass;
                }
                else
                {
                    passType = E_PassType.Fail;
                }
            }
            else
            {
                passType = E_PassType.Fail;
            }

            switch (passType)
            {
                case E_PassType.TraitPass:
                    //character.stamina--;    //UI changes to stamina bar should be done here
                    characterManager.AdjustStamina(character, -1);

                    StartCoroutine(TextTyper(string.Format(currentScenario.passTraitText, character.characterName)));
                    changeScenario = true;
                    break;

                case E_PassType.Secondary1Pass:
                    //character.stamina--;    //UI changes to stamina bar should be done here
                    characterManager.AdjustStamina(character, -1);

                    StartCoroutine(TextTyper(string.Format(currentScenario.secondary1PassText, character.characterName)));
                    changeScenario = true;
                    break;

                case E_PassType.Secondary2Pass:
                    //character.stamina--;    //UI changes to stamina bar should be done here
                    characterManager.AdjustStamina(character, -1);

                    StartCoroutine(TextTyper(string.Format(currentScenario.secondary2PassText, character.characterName)));
                    changeScenario = true;
                    break;

                case E_PassType.LuckyPass:
                    //character.stamina--;    //UI changes to stamina bar should be done here
                    characterManager.AdjustStamina(character, -1);

                    StartCoroutine(TextTyper(string.Format(currentScenario.luckyPassText, character.characterName)));
                    changeScenario = true;
                    break;

                case E_PassType.Fail:
                    if (character.health > 0)
                    {
                        StartCoroutine(TextTyper(string.Format(currentScenario.failText, character.characterName)));
                        characterManager.AdjustStamina(character, -1);
                        characterManager.AdjustHealth(character, -1);
                        if (character.health == 0)
                            partyMembersDown++;
                        changeScenario = true;
                    }
                    else
                    {
                        if (partyMembersDown < 3)
                        {
                            StartCoroutine(TextTyper(string.Format(currentScenario.characterCriticalFail, character.characterName)));
                            changeScenario = true;
                        }
                        else
                        {
                            StartCoroutine(TextTyper(string.Format(currentScenario.partyCriticalFail, character.name)));
                            changeScenario = true;
                        }
                    }
                    break;
            }
        }
    }
}