using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scenario", menuName = "CreateNewScenario", order = 1)]
public class ScenarioSO : ScriptableObject
{
    [TextArea]
    public string scenarioText;

    [TextArea]
    public string passText;
    [TextArea]
    public string failText;

    [TextArea]
    public string characterCriticalFail;
    [TextArea]
    public string partyCriticalFail;

    public E_Trait traitToPass;
    
}
