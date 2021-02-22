using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ScenarioManager : MonoBehaviour
{
    public int amountOfScenarios;

    [SerializeField]
    private List<ScenarioSO> scenarioList = new List<ScenarioSO>();


    private Queue<ScenarioSO> scenarioQueue = new Queue<ScenarioSO>();
    private ScenarioSO currentScenario;
    private E_Trait lastTraidUsed;

    // Start is called before the first frame update
    void Start()
    {
        GenerateScenarioQueue();
        currentScenario = scenarioQueue.Peek();
    }

    private void GenerateScenarioQueue()
    {
        while (scenarioQueue.Count < amountOfScenarios)
        {
            ScenarioSO scenario = scenarioList[Random.Range(0, scenarioList.Count)];
            if (scenarioQueue != null)
            {
                if (scenario.traitToPass != lastTraidUsed)
                {
                    scenarioQueue.Enqueue(scenario);
                    lastTraidUsed = scenario.traitToPass;
                    print(scenario.name + "added to queue");
                }
            }
            else
            {
                scenarioQueue.Enqueue(scenario);
                lastTraidUsed = scenario.traitToPass;
                Debug.Log("Added " + scenario.name);
            }
        }
    }

    public void NextScenario()
    {
        //Fade transition 
        scenarioQueue.Dequeue();
        currentScenario = scenarioQueue.Peek();
    }
}
