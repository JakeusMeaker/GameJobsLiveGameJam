using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scenario", menuName = "CreateNewScenario", order = 1)]
public class ScenarioSO : ScriptableObject
{
    public Sprite backgroundImage;
    public Sprite foregroundImage;

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

    public E_Trait traitToPass; //80% Success Rate
    public E_Trait[] moderateTraits; //50% Success Rate
    
}
