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

    [SerializeField]
    private List<ScenarioSO> scenarioList = new List<ScenarioSO>();

    private Queue<ScenarioSO> scenarioQueue = new Queue<ScenarioSO>();
    private ScenarioSO currentScenario;
    private E_Trait lastTraidUsed;
    private bool changeScenario = false;

    [Header("Scenario UI Elements")]
    public Text scenarioTextBox;

    private int partyMembersDown = 0;

    public static Action<E_Trait, SO_Character> ATraitSelected;
    public static Action AStartScenario;

    // Start is called before the first frame update
    void Start()
    {
        ATraitSelected += TraitSelected;
        AStartScenario += StartCurrentScenario;
        GenerateScenarioQueue();
        currentScenario = scenarioQueue.Peek();
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
                if (scenario.traitToPass != lastTraidUsed)
                {
                    scenarioQueue.Enqueue(scenario);
                    lastTraidUsed = scenario.traitToPass;
                }
            }
            else
            {
                scenarioQueue.Enqueue(scenario);
                lastTraidUsed = scenario.traitToPass;
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
        yield return new WaitForSeconds(1.5f);
        scenarioTextBox.text = "";
        LoadNextScenario();
        BlackoutAnimator.instance.FadeFromBlack();
    }

    public void LoadNextScenario()
    {
        if (scenarioQueue.Count > 0)
        {
            scenarioQueue.Dequeue();
            currentScenario = scenarioQueue.Peek();
            Invoke("StartCurrentScenario", scenarioChangeTime);
        }
        else
        {
            //End game condititions here 

        }
    }

    public void StartCurrentScenario()
    {
        if (currentScenario.backgroundImage != null)
            backgroundSprite.sprite = currentScenario.backgroundImage;
        if (currentScenario.foregroundImage != null)
            foregroundSprite.sprite = currentScenario.foregroundImage;

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

        if (trait == scenarioQueue.Peek().traitToPass)
        {
            StartCoroutine(TextTyper(string.Format(currentScenario.passText, character.characterName)));
            changeScenario = true;
        }
        else
        {
            if (character.stamina > 0)
            {
                StartCoroutine(TextTyper(string.Format(currentScenario.failText, character.characterName)));
                character.stamina--;
                if (character.stamina == 0)
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
        }
    }
}