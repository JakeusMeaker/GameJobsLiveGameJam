﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioManager : MonoBehaviour
{
    public SpriteRenderer backgroundSprite;
    public SpriteRenderer foregroundSprite;

    public bool _DebugBool;

    [Header("Scenario Generation"), Range(3, 20)]
    public int amountOfScenarios;

    //[Range(0.5f, 10f)]
    //public float scenarioChangeTime;

    public int healthGainInRestRoomAmount;
    public int staminaGainInRestRoomAmount;

    float textSpeed = 0.05f;

    [SerializeField]
    private List<ScenarioSO> scenarioList = new List<ScenarioSO>();

    public ScenarioSO restRoom;
    public ScenarioSO firstScenario;
    public ScenarioSO lastScenario;

    public FinalScene finalSceneScript;

    private Queue<ScenarioSO> scenarioQueue = new Queue<ScenarioSO>();
    private ScenarioSO currentScenario;

    private bool enableContinueButton = false;
    string currentText;

    [Header("Scenario UI Elements")]
    public Text scenarioTextBox;
    public GameObject restRoomContinueButton;
    public GameObject endGameButton;

    private int partyMembersDown = 0;

    public static Action<E_Trait, SO_Character> ATraitSelected;
    public static Action AStartScenario;

    [Header("Trait Percentages"), Range(0f, 1f)]
    public float secondaryTraitSuccessPercent;
    private E_PassType passType;

    private CharacterManager characterManager = null;
    private bool canUseTrait = false;
    //private bool skipText = false;
    private bool isTyping = false;

    //Timer for Space Spam
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        characterManager = GetComponent<CharacterManager>();
        ATraitSelected += TraitSelected;
        AStartScenario += StartCurrentScenario;
        GenerateScenarioQueue();
        currentScenario = scenarioQueue.Peek();
    }


    private void OnDisable()
    {
        ATraitSelected -= TraitSelected;
        AStartScenario -= StartCurrentScenario;
    }

    /* MAGNIFICENT LIST OF BUGS FOR JAKE (LOVE YOU JAKE)
     WHen spamming space + left click, can still fuck up the typing robot (Maybe fixed?) - yeah haven't been able to replicate 
    Also seem to end up on first scene again when doing that - seems to be fixed. I had added it within the loop and I think it was causing issues
    also need to disable buttons on characters who are exhausted or dead
    also need ending scene
    also jeez this is a lot
     */


    private void GenerateScenarioQueue()
    {
        scenarioQueue.Enqueue(firstScenario);

        if (_DebugBool)
        {
            scenarioQueue.Enqueue(lastScenario);
            return;
        }
        else
        {
            while (scenarioQueue.Count < amountOfScenarios - 1)
            {
                //This was for if only 1 scenario was in the scenario list
                if (scenarioList.Count == 1)
                {
                    scenarioQueue.Enqueue(scenarioList[0]);
                    return;
                }

                ScenarioSO scenario = scenarioList[UnityEngine.Random.Range(0, scenarioList.Count)];
                if (scenarioQueue != null)
                {
                    // This was supposed to create a rest room for the mid game but caused too many issues
                    if (!scenarioQueue.Contains(scenario))
                        scenarioQueue.Enqueue(scenario);
                    //{
                    //    if (scenarioQueue.Count == (amountOfScenarios * 0.5f) + 1)
                    //        scenarioQueue.Enqueue(restRoom);
                    //    else
                    //}
                }
                else
                {
                    scenarioQueue.Enqueue(scenario);
                }
            }
        }

        scenarioQueue.Enqueue(lastScenario);
    }

    public void NextScenario()
    {

        StopAllCoroutines();
        canUseTrait = false;
        scenarioTextBox.text = "";
        StartCoroutine(IE_LoadNextScene());
    }

    IEnumerator IE_LoadNextScene()
    {
        BlackoutAnimator.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        scenarioTextBox.text = "";
        LoadNextScenario();
        BlackoutAnimator.instance.FadeFromBlack();
    }

    public void LoadNextScenario()
    {
        scenarioQueue.Dequeue();

        if (scenarioQueue.Count > 0)
        {
            currentScenario = scenarioQueue.Peek();

            if (currentScenario == lastScenario)
            {
                SetContinueButton(false);
                canUseTrait = false;
                StartCurrentScenario();
                endGameButton.SetActive(true);
                return;
            }

            StartCurrentScenario();
        }
    }

    public void EndGame()
    {
        scenarioTextBox.text = "";
        finalSceneScript.DoFinalScene(characterManager.selectedCharacters.ToArray(), true);
        endGameButton.SetActive(false);
    }

    void SkipText()
    {
        StopAllCoroutines();
        isTyping = false;
        scenarioTextBox.text = "";
        scenarioTextBox.text = currentText;
        if (enableContinueButton == true)
        {
            SetContinueButton(true);
        }
        else
            SetContinueButton(false);

        if (currentScenario.isRestRoom == true)
        {
            SetContinueButton(true);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        //if (timer > 2)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space) && isTyping)
        //    {
        //        SkipText();
        //        timer = 0;
        //    }
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            textSpeed = 0.001f;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            textSpeed = 0.05f;
        }


        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    finalSceneScript.DoFinalScene(characterManager.selectedCharacters.ToArray(), true);
        //}
    }

    public void StartCurrentScenario()
    {
        if (currentScenario.backgroundImage != null)
            backgroundSprite.sprite = currentScenario.backgroundImage;
        else
            backgroundSprite.sprite = null;

        if (currentScenario.foregroundImage != null)
            foregroundSprite.sprite = currentScenario.foregroundImage;
        else
            foregroundSprite.sprite = null;

        //if (currentScenario.isRestRoom == true)
        //    SetContinueButton(true);
        //else
            SetContinueButton(false);

        StopCoroutine("TextTyper");
        if (!isTyping)
        {
            StartCoroutine(TextTyper(currentScenario.scenarioText, false));
        }
    }

    IEnumerator TextTyper(string textToType, bool isContinue)
    {
        isTyping = true;
        currentText = textToType;
        if (isContinue)
        {
            enableContinueButton = true;
            canUseTrait = false;
        }
        else
        {
            enableContinueButton = false;
            canUseTrait = true;
        }

        if (currentScenario.isRestRoom)
        {
            canUseTrait = false;
        }

        scenarioTextBox.text = "";
        for (int i = 0; i < textToType.Length; i++)
        {
            scenarioTextBox.text += textToType[i];
            yield return new WaitForSeconds(textSpeed);
        }

        if (isContinue || currentScenario.isRestRoom == true)
        {
            SetContinueButton(true);
        }
        isTyping = false;
    }

    public void TraitSelected(E_Trait trait, SO_Character character)
    {
        if (canUseTrait)
        {
            StopAllCoroutines();
            canUseTrait = false;
            scenarioTextBox.text = "";

            for (int i = 0; i < characterManager.selectedCharacters.Count; i++)
            {
                if (character != characterManager.selectedCharacters[i])
                {
                    RegenerateStamina(characterManager.selectedCharacters[i]);
                }
            }

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
                    characterManager.AdjustStamina(character, -1);    //UI changes to stamina bar should be done here
                    AudioManager.instance.Play(E_SFX.Success);

                    if (character.health == 0)
                        StartCoroutine(TextTyper(string.Format(currentScenario.passTraitText + "\nUnfortuantely, {0} is exhausted from the effort and collapses. They won't be able to continue.", character.characterName), true));
                    else
                        StartCoroutine(TextTyper(string.Format(currentScenario.passTraitText, character.characterName), true));

                    break;

                case E_PassType.Secondary1Pass:
                    characterManager.AdjustStamina(character, -1);    //UI changes to stamina bar should be done here
                    AudioManager.instance.Play(E_SFX.Success);

                    if (character.health == 0)
                        StartCoroutine(TextTyper(string.Format(currentScenario.secondary1PassText + "\nUnfortuantely, {0} is exhausted from the effort and collapses. They won't be able to continue.", character.characterName), true));
                    else
                        StartCoroutine(TextTyper(string.Format(currentScenario.secondary1PassText, character.characterName), true));
                    //SetContinueButton(true);
                    break;

                case E_PassType.Secondary2Pass:
                    characterManager.AdjustStamina(character, -1);    //UI changes to stamina bar should be done here
                    AudioManager.instance.Play(E_SFX.Success);
                    if (character.health == 0)
                        StartCoroutine(TextTyper(string.Format(currentScenario.secondary2PassText + "\nUnfortuantely, {0} is exhausted from the effort and collapses. They won't be able to continue.", character.characterName), true));
                    else
                        StartCoroutine(TextTyper(string.Format(currentScenario.secondary2PassText, character.characterName), true));
                    break;

                case E_PassType.LuckyPass:
                    characterManager.AdjustStamina(character, -1);    //UI changes to stamina bar should be done here
                    AudioManager.instance.Play(E_SFX.Success);

                    if (character.health == 0)
                        StartCoroutine(TextTyper(string.Format(currentScenario.luckyPassText + "\nUnfortuantely, {0} is exhausted from the effort and collapses. They won't be able to continue.", character.characterName), true));
                    else
                        StartCoroutine(TextTyper(string.Format(currentScenario.luckyPassText, character.characterName), true));
                    // SetContinueButton(true);
                    break;

                case E_PassType.Fail:

                    //Types too early, needs to adjust health and then sort appropriate respooinse. 
                    // character.isDead is implemented now, that would work great.
                    // Could swap int of dead characters for a loop that checks for not dead before game over?
                    // Love you

                    characterManager.AdjustStamina(character, -1);
                    characterManager.AdjustHealth(character, -1);

                    if (character.health > 0)
                    {
                        StartCoroutine(TextTyper(string.Format(currentScenario.failText, character.characterName), true));
                        AudioManager.instance.Play(E_SFX.Injury);
                    }
                    else
                    {
                        if (character.health == 0)
                        {
                            partyMembersDown++;
                            AudioManager.instance.Play(E_SFX.Death);
                            //TODO : Kill Character here - Sprite Remove, Etc.

                            if (partyMembersDown < 3)
                            {
                                StartCoroutine(TextTyper(string.Format(currentScenario.characterCriticalFail, character.characterName), true));
                                // SetContinueButton(true);
                                characterManager.KillCharacter(character);
                            }
                            else
                            {
                                StartCoroutine(TextTyper(string.Format(currentScenario.partyCriticalFail, character.name), true));
                                // SetContinueButton(true);
                                //Errrr game over stuff then I guess? 
                                finalSceneScript.DoFinalScene(characterManager.selectedCharacters.ToArray(), false);
                            }
                        }
                        else
                            AudioManager.instance.Play(E_SFX.Injury);
                        //SetContinueButton(true);

                    }
                    break;
            }

        }
    }

    void RegenerateStamina(SO_Character _char)
    {
        int _chance = UnityEngine.Random.Range(0, 100);
        if (_chance > 40)
        {
            characterManager.AdjustStamina(_char, +1);
        }
    }

    public void SetContinueButton(bool _state)
    {
        restRoomContinueButton.SetActive(_state);
    }
}